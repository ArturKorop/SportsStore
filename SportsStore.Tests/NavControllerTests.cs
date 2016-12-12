using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;
using SportsStore.WebUI.Controllers;

namespace SportsStore.Tests
{
    [TestFixture]
    public class NavControllerTests
    {
        [Test]
        public void Can_Create_Categories()
        {
            var repo = Substitute.For<IProductRepository>();
            repo.Products.Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Apples"},
                new Product {ProductId = 2, Name = "P2", Category = "Apples"},
                new Product {ProductId = 3, Name = "P3", Category = "Plums"},
                new Product {ProductId = 4, Name = "P4", Category = "Oranges"},
            });

            var controller = new NavController(repo);

            var result = ((IEnumerable<string>) controller.Menu().Model).ToArray();

            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0], Is.EqualTo("Apples"));
            Assert.That(result[1], Is.EqualTo("Oranges"));
            Assert.That(result[2], Is.EqualTo("Plums"));
        }
    }
}