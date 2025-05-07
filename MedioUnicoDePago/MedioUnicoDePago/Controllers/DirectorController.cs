using log4net;
using MedioUnicoDePago.Ayudante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedioUnicoDePago.Controllers
{
    public class DirectorController : ControladorBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DirectorController));
        public ActionResult StartApp()
        {
            //log.Info("Controller Director --> StarApp");
            //if (Request.Cookies["CUIL"] == null)
            //{
            //    log.Info("Controller Director --> Cookie.ObtenerCookie(Request, 'CUIL') == null");
            //    return base.RedirectToAction("Index", "IngresoCuil", new { area = "Desa" });
            //}
            //log.Info("Controller Director --> Cookie.ObtenerCookie(Request, 'CUIL') != null");
            //var value = Convert.ToDecimal(Cookie.ObtenerCookie(Request, "CUIL"));
            return base.RedirectToAction("Index", "Home");
        }
    }
}