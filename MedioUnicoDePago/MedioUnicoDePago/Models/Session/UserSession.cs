using Anses.Director.Session;
using System;
using System.Linq;
using MedioUnicoDePago.Ayudante;

namespace MedioUnicoDePago.Models.Session
{
    public class UserSession
    {
        public SSOToken credenciales { get; set; }

        public UserSession()
        {

        }

        public Usuario ObtenerDatosCredencial()
        {
            try
            {
                var cred = Credencial.ObtenerCredencial();
                if (cred != null)
                {
                    credenciales = cred.SSOToken;
                    var user = new Usuario();
                    user.Perfil = credenciales.Operation.Login.Groups[0].Name;
                    user.CUIL = credenciales.Operation.Login.CUIL;
                    user.UserId = credenciales.Operation.Login.UId;
                    user.UserName = credenciales.Operation.Login.Info[0].Value;
                    var oficina = credenciales.Operation.Login.Info.SingleOrDefault(r => r.Name.Equals("oficina", StringComparison.CurrentCultureIgnoreCase));
                    user.Oficina = oficina == null ? string.Empty : oficina.Value;
                    var oficinaDetalle = credenciales.Operation.Login.Info.SingleOrDefault(r => r.Name.Equals("oficinadesc", StringComparison.CurrentCultureIgnoreCase));
                    user.OficinaDetalle = oficinaDetalle == null ? string.Empty : oficinaDetalle.Value;
                    var ip = credenciales.Operation.Login.Info.SingleOrDefault(r => r.Name.Equals("ip", StringComparison.CurrentCultureIgnoreCase));
                    user.IP = ip == null ? string.Empty : ip.Value;
                    user.ExpiraToken = Credencial.ObtenerCredencial().expirasession;
                    user.HasToken = true;

                    user.Sistema = credenciales.Operation.Login.System;
                    user.Grupos = DirectorHelper.GetTokenListaGroups(credenciales);

                    return user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}