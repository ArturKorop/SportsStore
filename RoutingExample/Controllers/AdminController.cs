using System.Web.Mvc;

namespace RoutingExample.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Controller = "Admin";
            ViewBag.Action = "Index";
            ViewBag.Route = RouteData.Values["id"];
            return View("ActionName");
        }
    }
}