using MedioUnicoDePago.Models.Session;
using System.Web.Mvc;

namespace MedioUnicoDePago.Controllers
{
    public class AnsesController : Controller
    {
        public Usuario UsuarioSession
        {
            get
            {
                if (Session["UsuarioSession"] == null)
                    Session["UsuarioSession"] = new Usuario();
                return Session["UsuarioSession"] as Usuario;
            }
            set { Session["UsuarioSession"] = value; }
        }
    }
}