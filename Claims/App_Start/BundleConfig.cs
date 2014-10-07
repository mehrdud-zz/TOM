using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ClaimsPoC.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Content/Scripts/jquery.unobtrusive*",
            "~/Content/Scripts/jquery.validate*"));


            //bundles.Add(new ScriptBundle("~/js/required").Include(
            //        "~/Content/Scripts/jquery.min.js"));

            //bundles.Add(new ScriptBundle("~/js/required").Include(
            //        "~/Content/Scripts/jquery-ui.js"));


            //// The Kendo JavaScript bundle
            //bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
            //        "~/Content/Scripts/kendo.web.min.js", 
            //        "~/Content/Scripts/kendo.aspnetmvc.min.js"));

            //bundles.Add(new StyleBundle("~/Content/kendostyles").Include(
            //                "~/Content/Styles/kendo.common.*",
            //                "~/Content/Styles/kendo.default.*",
            //                "~/Content/Styles/kendo.dataviz.*"
            //                ));

            // Clear all items from the ignore list to allow minified CSS and JavaScript files in debug mode
            bundles.IgnoreList.Clear();


            // Add back the default ignore list rules sans the ones which affect minified files and debug mode
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);

        }
    }
}