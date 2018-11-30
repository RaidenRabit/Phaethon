using System.Web;
using System.Web.Optimization;

namespace WebClient
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region ScriptBundles
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-{version}.min.js",
                        "~/Scripts/jquery-ui-1.12.1.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Invoice").Include(
                        "~/Scripts/moment.min.js",
                        "~/Scripts/daterangepicker.js",
                        "~/Scripts/invoice.index.js"));

            bundles.Add(new ScriptBundle("~/bundles/Invoice/Edit").Include(
                "~/Scripts/invoice.edit.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-select.js",
                      "~/Scripts/respond.js"));
            #endregion

            #region StyleBundles
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-select.css",
                      "~/Content/themes/base/jquery-ui.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Daterangepicker").Include(
                      "~/Content/daterangepicker.css"));
            #endregion

            bundles.Add(new ScriptBundle("~/bundles/Job").Include(
                "~/Scripts/moment.min.js",
                "~/Scripts/job.index.js",
                "~/Scripts/daterangepicker.js"));
        }
    }
}
//"~/Content/bootstrap-select.css",
//"~/Scripts/umd/popper.js",
//"~/Scripts/bootstrap-select.js",