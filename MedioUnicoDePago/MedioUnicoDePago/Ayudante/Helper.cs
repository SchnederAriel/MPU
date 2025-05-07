using System;
using System.Linq;
using System.Net;
using Anses.Director.Session;

namespace MedioUnicoDePago.Ayudante
{
    public class Helper
    {
        /// <summary>
        /// Obtiene la ip del equipo que esta ejecutando
        /// </summary>
        /// <returns>Ip del equipo</returns>
        public static string GetIp()
        {
            var ip4Address = String.Empty;
            foreach (var ipa in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ipa.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) continue;
                ip4Address = ipa.ToString();
                break;
            } return
            ip4Address;
        }

        /// <summary>
        /// Obtiene el legajo del token
        /// </summary>
        /// <returns>String con el legajo </returns>
        public static string getUsrLegajo()
        {

            return Credencial.ObtenerCredencial().SSOToken.Operation.Login.UId;
        }

        /// <summary>
        /// Obtiene el nombre completode oficina del token
        /// </summary>
        /// <returns>String con el nombre completo</returns>
        public static string getTokenNombreUsuario()
        {
            const string clave = "nombre";
            return buscarInfoEnToken(clave);
        }

        /// <summary>
        /// Obtiene la descipcion de oficina del token
        /// </summary>
        /// <returns>String con la descipcion de oficina</returns>
        public static string getTokenDependencia()
        {
            const string clave = "oficinadesc";
            return buscarInfoEnToken(clave);
        }

        /// <summary>
        /// Obtiene el codigo de oficina del token
        /// </summary>
        /// <returns>String con el codigo de oficina</returns>
        public static string getTokenCodDependencia()
        {
            const string clave = "oficina";
            return buscarInfoEnToken(clave);
        }

        /// <summary>
        /// Obtiene el e-mail del token
        /// </summary>
        /// <returns>String con el e-mail</returns>
        public static string getTokenEmail()
        {
            const string clave = "email";
            return buscarInfoEnToken(clave);
        }

        /// <summary>
        /// Obtiene la IP del token
        /// </summary>
        /// <returns>String con la IP</returns>
        public static string getTokenIp()
        {
            const string clave = "ip";
            return buscarInfoEnToken(clave);
        }

        /// <summary>
        /// Obtiene el depertamente del token
        /// </summary>
        /// <returns>String con el Departamento</returns>
        public static string getTokenDpto()
        {
            const string clave = "department";
            return buscarInfoEnToken(clave);
        }

        /// <summary>
        /// Busca informacion en el token
        /// </summary>
        /// <param name="clave">Nombre de la clave buscada</param>
        /// <returns></returns>
        private static string buscarInfoEnToken(string clave)
        {
            foreach (var item in Credencial.ObtenerCredencial().SSOToken.Operation.Login.Info)
            {
                if (item.Name.ToLower().Equals(clave.ToLower()))
                {
                    return item.Value;
                }
            }
            return "";
        }

        public static string[] getTokenListaGroups()
        {
            var token = Credencial.ObtenerCredencial().SSOToken;
            var Grupo = new string[token.Operation.Login.Groups.Length];
            var i = 0;
            foreach (var grupo in token.Operation.Login.Groups)
            {
                Grupo[i] = grupo.Name;
                ++i;
            }
            return Grupo;
        }

        public static string getTokenGroups()
        {
            return Credencial.ObtenerCredencial().SSOToken.Operation.Login.Groups.Aggregate("", (current, grupo) => string.Concat(current, grupo.Name));
        }

        /// <summary>
        /// Obtiene el token
        /// </summary>
        /// <returns>String con el token</returns>
        public static string getToken()
        {
            return Credencial.ObtenerCredencial().SSOToken.Operation.Login.System;
        }
    }
}
