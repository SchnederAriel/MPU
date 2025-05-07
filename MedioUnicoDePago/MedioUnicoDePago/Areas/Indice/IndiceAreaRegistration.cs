using System.Web.Mvc;

namespace CunaMiAnses.Areas.Indice
{
    public class IndiceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Indice";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Indice_default",
                "Indice/{controller}/{action}/{id}",
                new { action = "Indice", id = UrlParameter.Optional }
            );
        }
    }
}