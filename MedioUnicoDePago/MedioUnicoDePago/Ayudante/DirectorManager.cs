using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Anses.Director.Session;
using System.Linq;
using log4net;

namespace MedioUnicoDePago.Ayudante
{

    public static class DirectorManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DirectorManager));

        [Serializable]
        public struct DirectorData
        {
            public string servicio;
            public string accion;
            public string soapFile;
        }

        #region Geters & Seters
        private static System.Web.SessionState.HttpSessionState Session
        {
            get
            {
                return (System.Web.SessionState.HttpSessionState)HttpContext.Current.Session;
            }
        }
        public static List<String> DirGroups
        {
            get
            {
                if (Session["__directorGroups"] != null)
                    return (List<String>)Session["__directorGroups"];
                SSOToken oCredencial = Credencial.ObtenerCredencial().SSOToken;

                DirGroups = new List<String>();
                foreach (Anses.Director.Session.GroupElement groupItem in oCredencial.Operation.Login.Groups)
                {
                    DirGroups.Add(groupItem.Name.ToString());
                }
                return (List<String>)Session["__directorGroups"];
            }
            private set
            {
                if (value == null)
                    ListaPermisos = null;
                else
                {
                    if (Session["__directorGroups"] != null)
                    {
                        if (DirGroups.Count != value.Count)
                            ListaPermisos = null;
                        else
                        {
                            if (!value.TrueForAll(delegate (string grupoAmatchear)
                            {
                                return DirGroups.Exists(
                                        delegate (string itemGrupo)
                                        {
                                            return itemGrupo == grupoAmatchear;
                                        }
                                        );

                            }))
                                ListaPermisos = null;
                        }

                    }
                }

                Session["__directorGroups"] = value;
            }
        }
        public static string DirSystem
        {
            get
            {
                if (Session["__directorSystem"] != null)
                    return (string)Session["__directorSystem"];
                SSOToken oCredencial = Credencial.ObtenerCredencial().SSOToken;

                DirSystem = oCredencial.Operation.Login.System;// usuarioEnDirector.Sistema;

                return (string)Session["__directorSystem"];

            }
            private set
            {
                if (string.IsNullOrEmpty(value) ||
                    (!string.IsNullOrEmpty((string)Session["__directorSystem"]) && value != DirSystem))
                    ListaPermisos = null;

                Session["__directorSystem"] = value;
            }
        }
        public static List<DirectorData> ListaPermisos
        {
            get
            {
                if (Session["__listaPermisosDirector"] != null)
                    return (List<DirectorData>)Session["__listaPermisosDirector"];

                consultarDirector();

                return (List<DirectorData>)Session["__listaPermisosDirector"];
            }
            private set
            {
                Session["__listaPermisosDirector"] = value;
            }
        }
        #endregion

        #region Funciones
        #region ProcesarPermisos
        /// <summary>
        /// Procesa y ejecuta la acción pertinente a los objetos según los permisos obtenidos del director. 
        /// Se asume el nombre de la pagina que llamo como valor para el parametro DondeBuscarNombre.
        /// </summary>
        /// <param name="DondeBuscarCtrls">Control contenedor donde se encontraran los controles a ocultar/habilitar/etc</param>
        /// <param name="nested">Indica si busca el control de forma iterativa</param>
        public static void procesarPermisosControl(Control DondeBuscarCtrls, bool nested)
        {
            procesarPermisosControl(DondeBuscarCtrls,
                                    GetPageNameFromControl(DondeBuscarCtrls),
                                    nested);
        }

        /// <summary>
        /// Procesa y ejecuta la acción pertinente a los objetos según los permisos obtenidos del director
        /// </summary>
        /// <param name="DondeBuscarCtrls">Control contenedor donde se encontraran los controles a ocultar/habilitar/etc</param>
        /// <param name="DondeBuscarNombre">Nombre del "servicio" del director donde se especifican los controles a ocultar/habilitar/etc como "metodos"</param>
        /// <param name="nested">Indica si busca el control de forma iterativa</param>
        public static void procesarPermisosControl(Control DondeBuscarCtrls, string DondeBuscarNombre, bool nested)
        {
            if (DondeBuscarCtrls == null)
                return;

            if (ListaPermisos == null)
                return;

            ListaPermisos.ForEach(
                delegate (DirectorData dirItem)
                {
                    if (dirItem.servicio.ToLower().Trim() == DondeBuscarNombre.ToLower().Trim())
                        realizarAccion(nested ?
                                            FindControlRecursive(DondeBuscarCtrls, dirItem.accion) :
                                            DondeBuscarCtrls.FindControl(dirItem.accion),
                                       dirItem);

                }
                );
        }

        /// <summary>
        /// Encuentra un control de forma iterativa (es decir busca en parent y si parent no lo tiene, busca en cada control de parent)
        /// </summary>
        /// <param name="parent">Donde busco</param>
        /// <param name="controlId">Que busco</param>
        /// <returns></returns>
        public static Control FindControlRecursive(Control parent, string controlId)
        {
            if (controlId == parent.ID)
                return parent;

            foreach (Control ctrl in parent.Controls)
            {
                Control tmp = FindControlRecursive(ctrl, controlId);
                if (tmp != null)
                    return tmp;
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Fuerza la asignacion del system y lista de grupo enviado por parametro para lograr una consulta mas flexible
        /// </summary>
        /// <param name="system"></param>
        /// <param name="grupos"></param>
        public static void fuerza_System_y_Grupo(string system, List<string> grupos)
        {
            DirSystem = system;
            DirGroups = grupos;
        }
        /// <summary>
        /// Trae el permiso de existir
        /// </summary>
        /// <param name="permiso">Que busco</param>
        /// <param name="control">Control del cual se deduce la pagina</param>
        /// <returns></returns>
        public static DirectorData? traerPermiso(string permiso, Control control)
        {
            return traerPermiso(permiso, GetPageNameFromControl(control));
        }

        /// <summary>
        /// Trae el permiso de existir
        /// </summary>
        /// <param name="permiso">Que busco</param>
        /// <param name="DondeBuscarNombre">En donde lo estoy buscando</param>
        /// <returns></returns>
        public static DirectorData? traerPermiso(string permiso, string DondeBuscarNombre)
        {
            if (ListaPermisos == null)
                return null;

            DirectorData p = ListaPermisos.Find(delegate (DirectorData dirItem)
            {
                return (dirItem.servicio.Trim() == DondeBuscarNombre && dirItem.accion == permiso);
            }
                );

            if (p.accion == null && p.servicio == null && p.soapFile == null)
                return null;
            return p;
        }

        public static string GetPageNameFromControl(Control control)
        {
            return control.Page.Request.FilePath.Substring(control.Page.Request.FilePath.LastIndexOf("/") + 1);
        }

        private static void consultarDirector()
        {
            string sStackLog = string.Empty;

            try
            {
                sStackLog += "Se dimensiona el servicio\n";

                //WSGrantForSystemCache dirWs = new WSGrantForSystemCache();

                sStackLog += "Seteo de la url\n";
                //dirWs.Url = ConfigurationManager.AppSettings["WSGrantForSystem.WSGrantForSystem"];

                //sStackLog += "Url Seteada a: [" + dirWs.Url + "]\n";

                //sStackLog += "Seteo credenciales\n";

                //dirWs.Credentials = System.Net.CredentialCache.DefaultCredentials;
                ListaPermisos = new List<DirectorData>();
                DirectorData dirItem;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "\n MyStack:\n" + sStackLog);
            }
        }
        private static void realizarAccion(Control ctrl, DirectorData dir)
        {
            /*
            PREFIJO EN EL METODO
            ====================
            pnl: Panel
            btn: Button
            ddl: DropDownList
            lnk: LinkButton

            else=>no accion
            */

            if (ctrl == null)
                return;

            try
            {
                switch (dir.accion.Remove(dir.accion.IndexOf("_")).ToLower())
                {
                    case "pnl":
                        {
                            Panel obj = (Panel)ctrl;
                            if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "enable")
                                obj.Enabled = true;
                            else
                                obj.Visible = true;

                            break;
                        }
                    case "btn":
                        {
                            Button obj = (Button)ctrl;

                            if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "enable")
                                obj.Enabled = true;
                            else
                                obj.Visible = true;

                            break;
                        }
                    case "lnk":
                        {
                            LinkButton obj = (LinkButton)ctrl;
                            if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "enable")
                                obj.Enabled = true;
                            else
                                obj.Visible = true;

                            break;
                        }
                    case "ddl":
                        {
                            DropDownList obj = (DropDownList)ctrl;
                            if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "enable")
                                obj.Enabled = true;
                            else
                                obj.Visible = true;

                            break;
                        }
                    case "ib":
                        {
                            ImageButton obj = (ImageButton)ctrl;
                            if (!String.IsNullOrEmpty(dir.soapFile) && dir.soapFile.ToLower().Trim() == "enable")
                                obj.Enabled = true;
                            else
                                obj.Visible = true;

                            break;
                        }
                }
            }
            catch
            {
                // no hago nada
            }
        }

        internal static bool TienePermiso(string Valor, string page)
        {
            return DirectorManager.traerPermiso(Valor, page).HasValue;
        }

        //Funcion en 
        public static List<string> getListComponents(string token, string rol, string view, string tipoAccion)
        {//metodo para mvc

             return null;
        }
        #endregion
    }
}

