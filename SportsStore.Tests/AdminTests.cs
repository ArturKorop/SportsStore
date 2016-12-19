using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;
using SportsStore.WebUI.Controllers;

namespace SportsStore.Tests
{
    [TestFixture]
    public class AdminTests
    {
        private IProductRepository _repository;

        [SetUp]
        public void Init()
        {
            _repository = Substitute.For<IProductRepository>();
            _repository.Products.Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
            });
        }

        [Test]
        public void Index_Contains_All_Products()
        {
            var target = new AdminController(_repository);
            var result = ((IEnumerable<Product>) target.Index().ViewData.Model).ToArray();

            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("P1"));
            Assert.That(result[1].Name, Is.EqualTo("P2"));
            Assert.That(result[2].Name, Is.EqualTo("P3"));
        }

        [Test]
        public void Can_Edit_Product()
        {
            var target = new AdminController(_repository);

            var p1 = (Product)target.Edit(1).ViewData.Model;
            var p2 = (Product)target.Edit(2).ViewData.Model;
            var p3 = (Product)target.Edit(3).ViewData.Model;

            Assert.That(p1.ProductId, Is.EqualTo(1));
            Assert.That(p2.ProductId, Is.EqualTo(2));
            Assert.That(p3.ProductId, Is.EqualTo(3));
        }

        [Test]
        public void Cannot_Edit_Nonexistent_Product()
        {
            var target = new AdminController(_repository);

            var result = (Product)target.Edit(4).ViewData.Model;

            Assert.IsNull(result);
        }

        [Test]
        public void Can_Save_Valid_Changes()
        {
            var target = new AdminController(_repository);

            var product = new Product {Name = "Test"};

            var result = target.Edit(product);

            _repository.ReceivedWithAnyArgs().SaveProduct(Arg.Any<Product>());
            Assert.IsNotInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Cannot_Save_Invalid_Changes()
        {
            var target = new AdminController(_repository);

            var product = new Product {Name = "Test"};
            target.ModelState.AddModelError("error", "error");

            var result = target.Edit(product);

            _repository.DidNotReceiveWithAnyArgs().SaveProduct(Arg.Any<Product>());
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}