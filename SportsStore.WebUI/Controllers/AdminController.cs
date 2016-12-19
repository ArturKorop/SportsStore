using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;

namespace SportsStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _repository;

        public AdminController(IProductRepository repository)
        {
            _repository = repository;
        }

        public ViewResult Index()
        {
            return View(_repository.Products);
        }

        [HttpGet]
        public ViewResult Edit(int productId)
        {
            var product = _repository.Products.FirstOrDefault(x => x.ProductId == productId);

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveProduct(product);
                TempData["message"] = $"{product.Name} has been saved";

                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            var deletedProduct = _repository.DeleteProduct(productId);
            if(deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} was deleted";
            }

            return RedirectToAction("Index");
        }
    }
}