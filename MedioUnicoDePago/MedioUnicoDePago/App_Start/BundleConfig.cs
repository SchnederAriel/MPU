using System.Web;
using System.Web.Optimization;

namespace MedioUnicoDePago
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-migrate-3.0.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/funcionesglobales").Include(
                      "~/Scripts/Componentes/GlobalFn.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                        "~/Scripts/bootstrap-datepicker-anses.js",
                        "~/Scripts/locales/bootstrap-datepicker.es.min.js"));



            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
            "~/Scripts/inputmask/inputmask.js",
            "~/Scripts/inputmask/jquery.inputmask.js",
            "~/Scripts/inputmask/inputmask.extensions.js",
            "~/Scripts/inputmask/inputmask.date.extensions.js",
            //and other extensions you want to include
            "~/Scripts/inputmask/inputmask.numeric.extensions.js"));

            bundles.Add(new StyleBundle("~/Content/grilla").Include(
                     "~/Content/datatables.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/grilla").Include(
                      "~/Scripts/datatablesnet/jquery.dataTables.min.js",
                      "~/Scripts/datatablesnet/dataTables.rowGroup.min.js"));

            bundles.Add(new StyleBundle("~/Content/kendo").Include(
                     "~/Content/kendo.css"));

            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
                     "~/Content/all.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                      "~/Scripts/kendo/2016.1.412/kendo.all.min.js",
                      "~/Scripts/kendo/2016.1.412/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/Site.min.css"));

            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                      "~/Content/bootstrap-datepicker.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/impresion").Include(
                      "~/Scripts/jQuery.print.min.js"));

        }
    }
}
