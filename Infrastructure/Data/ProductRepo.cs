using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly StoreContext _context;
        public ProductRepo(StoreContext context)
        {
            _context = context;
        }
        
        public async Task DbMutationByProduct(Func<Product, Task> f, Product product)
        {
            await f(product);

            await _context.SaveChangesAsync();
        }

        public async Task DbMutationById(Func<int, Task> f, int id)
        {
            await f(id);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            return await Validate(id);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products
                        .Include(p => p.ProductBrand)
                        .Include(p => p.ProductType)
                        .ToListAsync();
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task DeleteProduct(int id)
        {
            _context.Products.Remove(await Validate(id));
        }


        public Task UpdateProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        private async Task<Product> Validate(int id)
        {
            var productFromDb = await _context.Products
                                        .Include(p => p.ProductBrand)
                                        .Include(p => p.ProductType)
                                        .FirstOrDefaultAsync(p => p.Id == id);
            if (productFromDb is null)
            {
                throw new ArgumentException();
            }
            return productFromDb;
        }

        public async Task<IEnumerable<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IEnumerable<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}