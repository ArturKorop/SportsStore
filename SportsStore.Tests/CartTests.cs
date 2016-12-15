using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.Tests
{
    [TestFixture]
    public class CartTests
    {
        [Test]
        public void Can_Add_New_Lines()
        {
            var p1 = new Product {ProductId = 1, Name = "P1"};
            var p2 = new Product {ProductId = 2, Name = "P2"};

            var target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            var results = target.Lines.ToArray();

            Assert.That(results.Length, Is.EqualTo(2));
            Assert.That(results[0].Product, Is.EqualTo(p1));
            Assert.That(results[1].Product, Is.EqualTo(p2));
        }

        [Test]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            var p1 = new Product { ProductId = 1, Name = "P1" };
            var p2 = new Product { ProductId = 2, Name = "P2" };

            var target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);

            var results = target.Lines.ToArray();

            Assert.That(results.Length, Is.EqualTo(2));
            Assert.That(results[0].Quantity, Is.EqualTo(11));
            Assert.That(results[1].Quantity, Is.EqualTo(1));
        }

        [Test]
        public void Can_Remove_Line()
        {
            var p1 = new Product { ProductId = 1, Name = "P1" };
            var p2 = new Product { ProductId = 2, Name = "P2" };

            var target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.RemoveLine(p1);

            var results = target.Lines.ToArray();

            Assert.That(results.Length, Is.EqualTo(1));
        }

        [Test]
        public void Calculate_Cart_Total()
        {
            var p1 = new Product { ProductId = 1, Name = "P1", Price = 100M};
            var p2 = new Product { ProductId = 2, Name = "P2", Price =  50M};

            var target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);

            var results = target.ComputeTotalValue();

            Assert.That(results, Is.EqualTo(450M));
        }

        [Test]
        public void Can_Clear_Contents()
        {
            var p1 = new Product { ProductId = 1, Name = "P1" };
            var p2 = new Product { ProductId = 2, Name = "P2" };

            var target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            target.Clear();

            var results = target.Lines.ToArray();
            Assert.That(results.Length, Is.EqualTo(0));
        }

        [Test]
        public void Can_Add_To_Cart()
        {
            var repo = Substitute.For<IProductRepository>();
            repo.Products.Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Apples"}
            });

            var cart =new Cart();
            var target = new CartController(repo);

            target.AddToCart(cart, 1, null);

            Assert.That(cart.Lines.Count(), Is.EqualTo(1));
            Assert.That(cart.Lines.ToArray()[0].Product.ProductId, Is.EqualTo(1));
        }

        [Test]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            var repo = Substitute.For<IProductRepository>();
            repo.Products.Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Apples"}
            });

            var cart = new Cart();
            var target = new CartController(repo);

            var result = target.AddToCart(cart, 1, "myUrl");

            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(result.RouteValues["returnUrl"], Is.EqualTo("myUrl"));
        }

        [Test]
        public void Can_View_Cart_Contents()
        {
            var cart = new Cart();
            var target = new CartController(null);

            var result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.That(result.Cart, Is.EqualTo(cart));
            Assert.That(result.ReturnUrl, Is.EqualTo("myUrl"));
        }

    }
}