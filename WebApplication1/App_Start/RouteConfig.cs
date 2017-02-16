using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Destination", url: "Circuit/Destination/{destination}",
                defaults: new { controller = "Circuit", action = "Destination", destination = UrlParameter.Optional });

            routes.MapRoute(
                name: "Interest", url: "Circuit/Interest/{interest}",
                defaults: new { controller = "Circuit", action = "Interest", interest = UrlParameter.Optional });

            routes.MapRoute(
               name: "Activity", url: "Circuit/Activity/{activity}",
               defaults: new { controller = "Circuit", action = "Interest", activity = UrlParameter.Optional });

            routes.MapRoute(
                name: "India-Destination", url: "India/Destination/{destination}",
                defaults: new { controller = "Circuit", action = "Destination", destination = UrlParameter.Optional });

            routes.MapRoute(
                name: "India-Interest", url: "India/Interest/{interest}",
                defaults: new { controller = "Circuit", action = "Interest", interest = UrlParameter.Optional });

            routes.MapRoute(
               name: "India-Activity", url: "India/Activity/{activity}",
               defaults: new { controller = "Circuit", action = "Interest", activity = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );            
        }
    }
}
