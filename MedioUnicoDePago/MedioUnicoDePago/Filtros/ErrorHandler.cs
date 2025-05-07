using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace MedioUnicoDePago.Filtros
{
    public class ErrorHandler : ActionFilterAttribute, IExceptionFilter
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ErrorHandler));

        public void OnException(ExceptionContext filterContext)
        {
            log.Info("---> ERROR controlado desde Error handler");

            HttpSessionStateBase sesion = filterContext.HttpContext.Session;
            Exception e = filterContext.Exception;
            string controllerName = (string)filterContext.RouteData.Values["controller"];
            string actionName = (string)filterContext.RouteData.Values["action"];
            string tipoError = filterContext.Exception.GetType().Name.ToUpper();

            log.Error(string.Format("Error tipo {0} en Contolador: {1} - Método: {2} : Error: {3} - StackTrace: {4}", tipoError, controllerName, actionName, e.Message, e.StackTrace));

            filterContext.ExceptionHandled = true;

            if (sesion != null && sesion["agenteUdai"] == null & tipoError.Equals("NULLREFERENCEEXCEPTION"))
            {

                //Redireccionar por fin de session pero no loggear
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Error" },
                    { "action", "ErrorSesion" },
                    { "area", string.Empty }
                });

            }
            else if (controllerName.ToUpper().Equals("HOME"))
            {

                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Error" },
                    { "action", "ErrorSolo" },
                    { "area", string.Empty },
                    { "mensaje", e.Message.Replace("\r","").Replace("\n","")}
                });

            }
            else if (e.Message.ToUpper().Equals("ACCESO DENEGADO"))
            {

                filterContext.Result = new ViewResult()
                {
                    ViewName = "ErrorEnLayout",
                    ViewData = new ViewDataDictionary(new ViewDataDictionary { { "mensaje", e.Message.Replace("\r", "").Replace("\n", "") }, { "denegado", true } })
                };

            }
            else
            {

                filterContext.Result = new ViewResult()
                {
                    ViewName = "ErrorEnLayout",
                    ViewData = new ViewDataDictionary(new ViewDataDictionary { { "mensaje", e.Message.Replace("\r", "").Replace("\n", "") }, { "denegado", false } })
                };
            }

        }
    }
}