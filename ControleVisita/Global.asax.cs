using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ControleVisita.Models;

namespace ControleVisita
{

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DevExtremeBundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
           // CultureInfo culture = CultureInfo.GetCultureInfo("pt-BR");
            //Thread.CurrentThread.CurrentUICulture = culture;
            //Thread.CurrentThread.CurrentCulture = culture;

            //var currentCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            //currentCulture.NumberFormat.NumberDecimalSeparator = ".";
            //currentCulture.NumberFormat.NumberGroupSeparator = " ";
            //currentCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            //Thread.CurrentThread.CurrentCulture = currentCulture;

            //if (Debugger.IsAttached)
            //    new LoginData().CreateCookie("VP");

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            //  var user = (LoginModel)HttpContext.Current.Session["USERMODEL"];
        }
    }
}
