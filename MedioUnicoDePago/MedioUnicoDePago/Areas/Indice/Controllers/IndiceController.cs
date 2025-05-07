using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CunaMiAnses.Areas.Indice.Controllers
{
    public class IndiceController : Controller
    {
        // GET: Indice/Indice
        public ActionResult Indice()
        {
            return View("Urls");
        }
    }
}