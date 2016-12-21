using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NSubstitute;
using SportsStore.WebUI.Infrastructure.Interfaces;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class AcountController : Controller
    {
        private IAuthProvider _authProvider;

        public AcountController(IAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                if(_authProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}