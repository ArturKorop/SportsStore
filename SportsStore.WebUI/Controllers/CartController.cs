using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IOrderProcessor _orderProcessor;

        public CartController(IProductRepository repository, IOrderProcessor orderProcessor)
        {
            _repository = repository;
            _orderProcessor = orderProcessor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl,
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(x => x.ProductId == productId);
            if(product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new {returnUrl});
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        [HttpGet]
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if(!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }

            if(ModelState.IsValid)
            {
                _orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();

                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }
    }
}