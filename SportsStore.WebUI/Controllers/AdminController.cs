using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
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
        public ActionResult Edit(Product product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if(image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }

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