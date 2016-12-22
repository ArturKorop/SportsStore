using System.Web.Mvc;

namespace RoutingExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";
            ViewBag.Route = RouteData.Values["id"];
            return View("ActionName");
        }
    }
}