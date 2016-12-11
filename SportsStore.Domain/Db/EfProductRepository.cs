using System.Collections.Generic;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Interfaces;

namespace SportsStore.Domain.Db
{
    public class EfProductRepository : IProductRepository
    {
        private readonly EfDbContext _context = new EfDbContext();

        public IEnumerable<Product> Products => _context.Products;
    }
}