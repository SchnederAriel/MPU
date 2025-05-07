using Anses.Director.Session;
using log4net;
using MedioUnicoDePago.Ayudante;
using MedioUnicoDePago.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedioUnicoDePago.Controllers
{
    public class ErrorController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ErrorController));

        // GET: ErrorSolo (NO debe tener layout)
        public ActionResult ErrorSolo(string mensaje)
        {
            log.Info("--->  traking en error controller");
            log.Info("---> ERROR: - " + mensaje);

            ViewData["mensaje"] = mensaje;            
            return View();
        }

        // GET: Caducó la sesion de usuario (NO debe tener layout)
        public ActionResult ErrorSesion()
        {         
            return View();
        }

    }
}