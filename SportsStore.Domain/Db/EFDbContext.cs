using System.Data.Entity;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Db
{
    public class EfDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}