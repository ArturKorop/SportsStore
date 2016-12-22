using System.Web.Mvc;
using System.Web.Routing;

namespace RoutingExample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("X", "X{controller}/{action}");
            routes.MapRoute("MyRoute1", "{controller}/{action}", new { action = "Index" });
            routes.MapRoute("MyRoute2", "{controller}/{action}/{id}", new {controller = "Home", action = "Index", id = "DefaultId"});
        }
    }
}
