using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
    }
}