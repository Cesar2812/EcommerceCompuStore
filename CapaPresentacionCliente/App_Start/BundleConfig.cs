using System.Web.Optimization;

namespace CapaPresentacionCliente
{
    public class BundleConfig
    {
       
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));



            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js",
                      "~/Scripts/fontawesome/all.mim.js",
                      "~/Scripts/loadingoverlay.mim.js",
                      "~/Scripts/sweetalert.min.js"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/styles.css",
                "~/Content/sweetalert.css"));
        }
    }
}
