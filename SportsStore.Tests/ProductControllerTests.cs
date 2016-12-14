using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        [Test]
        public void Can_Paginate()
        {
            var repo = Substitute.For<IProductRepository>();
            repo.Products.Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"},
            });

            var controller = new ProductController(repo) {PageSize = 3};

            var result = (ProductListViewModel) controller.List(null, 2).Model;
            var products = result.Products.ToArray();

            Assert.That(products.Length, Is.EqualTo(2));
            Assert.That(products[0].Name, Is.EqualTo("P4"));
            Assert.That(products[1].Name, Is.EqualTo("P5"));

            var pagingInfo = result.PagingInfo;
            Assert.That(pagingInfo.CurrentPage, Is.EqualTo(2));
            Assert.That(pagingInfo.ItemsPerPage, Is.EqualTo(3));
            Assert.That(pagingInfo.TotalPages, Is.EqualTo(2));
            Assert.That(pagingInfo.TotalItems, Is.EqualTo(5));
        }

        [Test]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper helper = null;
            var info = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> createPageUrl = i => "Page" + i;

            var result = helper.PageLinks(info, createPageUrl).ToString();

            var expected = @"<a class=""btn btn-default"" href=""Page1"">1</a>" +
                              @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" +
                              @"<a class=""btn btn-default"" href=""Page3"">3</a>";
            
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Can_Filter_Products()
        {
            var repo = Substitute.For<IProductRepository>();
            repo.Products.Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"},
            });

            var controller = new ProductController(repo) {PageSize = 3};

            var result = ((ProductListViewModel) controller.List("Cat2", 1).Model).Products.ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("P2"));
            Assert.That(result[1].Name, Is.EqualTo("P4"));
        }

        [Test]
        public void Generate_Category_Specific_Product_Count()
        {
            var repo = Substitute.For<IProductRepository>();
            repo.Products.Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"},
            });

            var controller = new ProductController(repo) { PageSize = 3 };

            Func<string, int> getTotalItems = s =>
            {
                var result = ((ProductListViewModel) controller.List(s).Model).PagingInfo.TotalItems;

                return result;
            };

            Assert.That(getTotalItems("Cat1"), Is.EqualTo(2));
            Assert.That(getTotalItems("Cat2"), Is.EqualTo(2));
            Assert.That(getTotalItems("Cat3"), Is.EqualTo(1));
        }
    }
}