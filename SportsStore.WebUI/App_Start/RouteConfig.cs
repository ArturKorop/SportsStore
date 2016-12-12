using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resources}.axd/{*pathInfo}");

            routes.MapRoute(name: null, url: "Page{page}", defaults: new {Controller = "Product", action = "List"});

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Product", action = "List", id = UrlParameter.Optional});
        }
    }
}