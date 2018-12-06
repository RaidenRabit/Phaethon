using System.Web;
using System.Web.Optimization;

namespace WebClient
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region ScriptBundle

            #region All pages
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-1.12.1.min.js"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",//plugin for full bootstrap experience
                "~/Scripts/respond.js"//script to enable responsive web designs in browsers that don’t support CSS3
            ));

            bundles.Add(new ScriptBundle("~/bundles/loading").Include(
                "~/Scripts/loadingAnimation.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/language").Include(
                "~/Scripts/changeLanguage.js"
            ));
            #endregion

            #region Page specific
            bundles.Add(new ScriptBundle("~/bundles/Invoice").Include(
                "~/Scripts/page-scripts/invoice.index.js"));

            bundles.Add(new ScriptBundle("~/bundles/Invoice/Edit").Include(
                "~/Scripts/page-scripts/invoice.edit_company.js",
                "~/Scripts/page-scripts/invoice.edit_elements.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/Item").Include(
                "~/Scripts/page-scripts/item.index.js"));

            bundles.Add(new ScriptBundle("~/bundles/Item/Edit").Include(
                "~/Scripts/page-scripts/item.edit.js"));

            bundles.Add(new ScriptBundle("~/bundles/Item/Select").Include(
                "~/Scripts/page-scripts/item.select.js"));

            bundles.Add(new ScriptBundle("~/bundles/Job").Include(
                "~/Scripts/page-scripts/job.index.js"));
            #endregion

            #region Use case specific
            bundles.Add(new ScriptBundle("~/bundles/dateRangePicker").Include(
                "~/Scripts/moment.min.js",//Parse, validate, manipulate, and display dates and times in JavaScript.
                "~/Scripts/dateRangePicker.js"//Date Range Picker can be attached to any webpage element to pop up two calendars for selecting dates, times, or predefined ranges
            ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapExtra").Include(
                "~/Scripts/umd/popper.js",
                "~/Scripts/bootstrap-select.js",
                "~/Scripts/respond.js"));
            #endregion

            #endregion

            #region StyleBundles

            #region All pages
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/HoverElement.css",
                "~/Content/bootstrap.css",
                "~/Content/themes/base/jquery-ui.css",
                "~/Content/font-awesome.css",
                "~/Content/site.css"));
            #endregion

            #region Use case specific
            bundles.Add(new StyleBundle("~/Content/dateRangePicker").Include(
                "~/Content/dateRangePicker.css"//Date Range Picker can be attached to any webpage element to pop up two calendars for selecting dates, times, or predefined ranges
            ));

            bundles.Add(new StyleBundle("~/Content/bootstrapExtra").Include(
                "~/Content/bootstrap-select.css"
            ));
            #endregion

            #endregion
        }
    }
}