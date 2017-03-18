using System.Web;
using System.Web.Optimization;

namespace WebApplication1
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/mainjs").Include(
                      "~/content/js/main.js",
                      "~/content/js/custom.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/css/styles").Include(
            //            "~/Content/css/animate.min.css",
            //            "~/Content/css/bootstrap.css",
            //            "~/Content/css/bootstrap.min.css",
            //            "~/Content/css/custom.css",
            //            "~/Content/assets/plugins/font-awesome/css/font-awesome.css", 
            //            "~/Content/css/font-awesome.min.css",
            //            "~/Content/css/magnific-popup.css",
            //            "~/Content/css/owl.carousel.css",
            //            "~/Content/css/parallax-slider.css",
            //            "~/Content/css/settings.css",
            //            "~/Content/css/smart-addons.css",
            //            "~/Content/css/smart-forms-ie8.css",
            //            "~/Content/css/smart-forms.css",
            //            "~/Content/css/styles/style.css",
            //            "~/Content/assets/css/pages/page_log_reg_v1.css",
            //            "~/Content/assets/css/pages/page_search.css",
            //            "~/Content/css/styles/skin-orange.css",
            //            "~/Content/assets/css/responsive.css"));


        }
    }
}


