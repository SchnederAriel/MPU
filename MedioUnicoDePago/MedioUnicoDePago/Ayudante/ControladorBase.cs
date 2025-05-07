using log4net;
using MedioUnicoDePago.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MedioUnicoDePago.Ayudante
{
    public class ControladorBase : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ControladorBase));

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    //No trapear error si ya estaba manejado desde global asax
        //    if (filterContext.ExceptionHandled)
        //        return;

        //    if (filterContext.HttpContext.Request.IsAjaxRequest())
        //    {
        //        Response.Write(filterContext.Exception.Message);
        //        Response.End();
        //    }
        //    else
        //    {
        //        log.Info("--->  ERROR manejado desde ControladorBase.cs");
        //        log.Error(string.Concat(filterContext.Exception, " ", filterContext.Exception.StackTrace));
        //        int indexStackTrace = (filterContext.Exception.StackTrace.Contains("\r")) ? filterContext.Exception.StackTrace.IndexOf("\r") : 60;
        //        filterContext.ExceptionHandled = true;

        //        //Redirigir a una vista de errores.
        //        var routeData = new RouteData();
        //        ErrorController errorController = new ErrorController();
        //        routeData.Values.Add("controller", "Error");
        //        routeData.Values.Add("action", "Error");

        //        if (filterContext.Exception.GetType().Name.Trim().ToUpper().Equals("NULLREFERENCEEXCEPTION"))
        //        {
        //            if (Session["datosUsuario"] is null)
        //                routeData.Values.Add("message", "Tiempo de inactividad superado. Vuelva a ingresar por ADP.");
        //            else
        //                routeData.Values.Add("message", filterContext.Exception.Message);
        //        }
        //        else
        //        {
        //            routeData.Values.Add("message", filterContext.Exception.Message);
        //        }

        //        routeData.Values.Add("type", filterContext.Exception.GetType() + filterContext.Exception.StackTrace.Substring(0, indexStackTrace));
        //        Response.StatusCode = 500;

        //        //Elimino contexto de la respuesta que provocó error.
        //        filterContext.HttpContext.Response.Clear();
        //        IController controller = new ErrorController();
        //        controller.Execute(new RequestContext(this.HttpContext, routeData));

        //    }

        //    base.OnException(filterContext);
        //}
    }
}