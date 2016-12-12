﻿using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Interfaces;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;

        public int PageSize  {get;set;}

        public ProductController(IProductRepository repository)
        {
            PageSize = 4;
            _repository = repository;
        }

        public ViewResult List(int page = 1)
        {
            var model = new ProductListViewModel
            {
                Products = _repository.Products
                .OrderBy(x => x.ProductId)
                .Skip((page - 1)*PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _repository.Products.Count()
                }
            };

            return View(model);
        }
    }
}