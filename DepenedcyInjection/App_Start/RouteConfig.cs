﻿using System.Web.Mvc;
using System.Web.Routing;

namespace DependencyInjection
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Filtering",
                url: "Filter/{fieldName}/{fieldValue}/{page}",
                defaults: new {controller = "Character", action = "Filter", page=1}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
