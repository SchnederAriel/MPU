using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MedioUnicoDePago.Models.Session;
using System.Configuration;

namespace MedioUnicoDePago.Filtros
{
    public class RequiereAutenticacionAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userSession = new UserSession();
            var usuario = userSession.ObtenerDatosCredencial();

            return usuario != null && usuario.HasToken;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["URLSTS"]);
        }
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    var userSession = new UserSession();

        //    try
        //    {


        //        var usuario = userSession.ObtenerDatosCredencial();
        //        if (usuario != null)
        //        {
        //            log.Error("SE PUDO OBTENER LOS DATOS DE LAS CREDENCIALES");
        //        }
        //        return usuario != null && usuario.HasToken;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("!!!!!!!!!!!!!ERROR AL OBTENER LOS DATOS DE LAS CREDENCIALES");
        //        log.Info($"ERROR AL OBTENER LOS DATOS DE LAS CREDENCIALES");
        //        return false;
        //    }

        //}
    }

}