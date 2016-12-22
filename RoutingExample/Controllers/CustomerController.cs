using System.Web.Mvc;

namespace RoutingExample.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Controller = "Customer";
            ViewBag.Action = "Index";
            ViewBag.Route = RouteData.Values["id"];
            return View("ActionName");
        }

        public ActionResult List()
        {
            ViewBag.Controller = "Customer";
            ViewBag.Action = "List";
            ViewBag.Route = RouteData.Values["id"];
            return View("ActionName");
        }
    }
}