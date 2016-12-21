using System.Linq;
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

        public ViewResult List(string category, int page = 1)
        {
            var model = new ProductListViewModel
            {
                Products = _repository.Products
                .Where(x=> category == null || x.Category == category)
                .OrderBy(x => x.ProductId)
                .Skip((page - 1)*PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null 
                    ? _repository.Products.Count()
                    : _repository.Products.Count(x => x.Category == category)
                }
            };

            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            var product = _repository.Products.FirstOrDefault(x => x.ProductId == productId);
            if(product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }

            return null;
        }
    }
}