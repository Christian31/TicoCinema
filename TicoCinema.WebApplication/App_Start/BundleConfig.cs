using System.Web;
using System.Web.Optimization;

namespace TicoCinema.WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/jquery.counterup.min.js",
                        "~/Scripts/jquery.parallax-{version}.js",
                        "~/Scripts/jquery.scrollTo.min.js",
                        "~/Scripts/jquery.waypoints.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

           bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/front").Include(
                        "~/Scripts/front.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-select.min.js"));
              
            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                      "~/Scripts/owl.carousel.min.js",
                      "~/Scripts/owl.carousel2.thumbs.min.js",
                      "~/Scripts/popper.min.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-select.min.css"));

            bundles.Add(new StyleBundle("~/Content/cssjqryui").Include(
                   "~/Content/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/vendors").Include(
                      "~/Content/font-awesome.min.css",
                      "~/Content/owl.carousel.css",
                      "~/Content/owl.theme.default.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
        }
    }
}
