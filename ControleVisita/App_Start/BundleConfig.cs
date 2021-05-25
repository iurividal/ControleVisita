using System.Web.Optimization;

namespace ControleVisita {

    public class BundleConfig {

        public static void RegisterBundles(BundleCollection bundles) {

            var scriptBundle = new ScriptBundle("~/Scripts/bundle");
            var styleBundle = new StyleBundle("~/Content/bundle");

            // jQuery
            scriptBundle
                .Include("~/Scripts/jquery-3.4.1.js");

            scriptBundle
                .Include("~/Scripts/jquery.mask.js");
           

            // jqeury validadte
            scriptBundle
                .Include("~/Scripts/bootstrap.js");

            scriptBundle
              .Include("~/Scripts/jquery.validate.js");
            
            scriptBundle
              .Include("~/Scripts/jquery.validate.pt-br.js");

            scriptBundle
           .Include("~/Scripts/Mascara.js");

            scriptBundle
           .Include("~/Scripts/toastr.js");

            // Bootstrap
            styleBundle
                .Include("~/Content/bootstrap.css");

            styleBundle
               .Include("~/Content/validate.css");

            // Custom site styles
            styleBundle
                .Include("~/Content/Site.css");

            styleBundle
                .Include("~/Content/toastr.css");





            bundles.Add(scriptBundle);
            bundles.Add(styleBundle);

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}