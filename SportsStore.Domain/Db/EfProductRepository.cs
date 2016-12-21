using System.Collections.Generic;
using System.Web.Mvc;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;

namespace SportsStore.Domain.Db
{
    public class EfProductRepository : IProductRepository
    {
        private readonly EfDbContext _context = new EfDbContext();

        public IEnumerable<Product> Products => _context.Products;
        public void SaveProduct(Product product)
        {
            if(product.ProductId == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                var entry = _context.Products.Find(product.ProductId);
                if(entry != null)
                {
                    entry.Name = product.Name;
                    entry.Description = product.Description;
                    entry.Price = product.Price;
                    entry.Category = product.Category;
                    entry.ImageData = product.ImageData;
                    entry.ImageMimeType = product.ImageMimeType;
                }
            }

            _context.SaveChanges();
        }

        public Product DeleteProduct(int productId)
        {
            var entry = _context.Products.Find(productId);
            if (entry != null)
            {
                _context.Products.Remove(entry);
                _context.SaveChanges();
            }

            return entry;
        }
    }
}