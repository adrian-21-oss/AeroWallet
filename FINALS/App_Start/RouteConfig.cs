using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace FINALS
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Tell Web Forms routing to stay away from the /api/ folder!
            //routes.Ignore("");
            //routes.Ignore("{resource}.axd/{*pathInfo}");
            //routes.Ignore("api/{*pathInfo}");

            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Off;  //BE changed if there was an error regarding to Error 402 or  401
            routes.EnableFriendlyUrls(settings);
        }
    }
}
