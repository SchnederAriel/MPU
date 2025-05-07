using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

using MedioUnicoDePago.Models.Session;

namespace MedioUnicoDePago.Controllers
{
    public class SessionController : AnsesController
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));
        [ChildActionOnly]
        public ActionResult GetAutenticationData()
        {
            try
            {
                if (!UsuarioSession.HasToken)
                {
                    //Limpio el menu de la sesion
                    Session["MenuDinamico"] = null;
                    UsuarioSession = new UserSession().ObtenerDatosCredencial();
                }
                return PartialView("~/Views/Shared/_Header.cshtml", UsuarioSession);
            }
            catch (Exception ex)
            {
                log.Error( "ERROR ObtenerDatosCredencial", ex);
                return RedirectToAction("/");
            }
        }

        [ChildActionOnly]
        public ActionResult GetMenuDinamico()
        {
            //fuerzo a cargar siempre el menu
           // Session["MenuDinamico"] = null;
            try
            {
                if (bool.Parse(ConfigurationManager.AppSettings["MenuDinamico"]))
                {
                    //Me fijo si el menu dinamico ya fue cargado
                    var menuDinamico = Session["MenuDinamico"];
                    if (menuDinamico == null)
                    {
                        if (UsuarioSession.Sistema != null && UsuarioSession.Grupos != null)
                        {
                            /*MenuDinamicoHelper srv = new MenuDinamicoHelper();
                            var menu = srv.ObtenerMenu(UsuarioSession.Sistema, UsuarioSession.Grupos[0]);
                            Session["MenuDinamico"] = menu;
                            return Content(menu);*/
                        }
                    }
                    else
                    {
                        return Content((string)Session["MenuDinamico"]);
                    }

                }
                //No se usa Menu Dinamico
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("ERROR Armar el menu", ex);
                return RedirectToAction("ServerError", "Error");
            }
            return null;
        }
    }
}