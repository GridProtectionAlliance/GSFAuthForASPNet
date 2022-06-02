using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GSF.Web;
using GSF.Web.Embedded;
using GSF.Web.Security;

namespace AuthTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Register common embedded resources
            WebExtensions.AddEmbeddedResourceAssembly(typeof(WebExtensions).Assembly);

            // Add additional virtual path provider to allow access to embedded resources
            EmbeddedResourceProvider.Register();
        }
    }
}
