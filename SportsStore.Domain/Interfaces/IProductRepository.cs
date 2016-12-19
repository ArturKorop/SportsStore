using System.Collections.Generic;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productId);
    }
}