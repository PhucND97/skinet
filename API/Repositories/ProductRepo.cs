using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly StoreContext _context;
        public ProductRepo(StoreContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();

        }
    }
}