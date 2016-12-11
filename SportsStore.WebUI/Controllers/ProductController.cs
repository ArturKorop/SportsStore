using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Interfaces;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;

        public int PageSize = 4;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        public ViewResult List(int page = 1)
        {
            return View(_repository.Products.OrderBy(x => x.ProductId).Skip((page - 1) * PageSize).Take(PageSize));
        }
    }
}