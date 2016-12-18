﻿using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Interfaces;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IProductRepository _repository;

        public NavController(IProductRepository repository)
        {
            _repository = repository;
        }

        public PartialViewResult Menu(string category = null, bool horizontalLayout = false)
        {
            ViewBag.SelectedCategory = category;
            var categories = _repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);

            var viewName = horizontalLayout ? "MenuHorizontal" : "Menu";

            return PartialView(viewName, categories);
        }
    }
}