﻿using System.Web.Mvc;
using System.Web.Routing;

namespace AuthTest
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "LoginRoute",
                url: "Login",
                defaults: new { controller = "Login", action = "Index" }
            );

            routes.MapRoute(
                name: "AuthTestRoute",
                url: "AuthTest",
                defaults: new { controller = "Login", action = "AuthTest" }
            );

            routes.MapRoute(
                name: "LogoutRoute",
                url: "Logout",
                defaults: new { controller = "Login", action = "Logout" }
            );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
