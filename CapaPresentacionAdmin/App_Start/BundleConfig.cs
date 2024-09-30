using System.Web.Optimization;

namespace CapaPresentacionAdmin
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            //bundles para la plantilla
            bundles.Add(new Bundle("~/bundles/complementos").Include(
                       "~/Scripts/scripts.js",
                        "~/Scripts/fontawesome/all.min.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.responsive.js",
                        "~/Scripts/loadingoverlay/loadingoverlay.min.js",
                        "~/Scripts/sweetalert2/sweetalert2.all.min.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/sweetalert.min.js"));


            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/DataTables/css/jquery.dataTables.css",
                "~/Content/DataTables/css/responsive.dataTables.css",
                "~/Content/sweetalert.css"
                ));
        }
    }
}
