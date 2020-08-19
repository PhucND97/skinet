using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepo
    {
        Task DbMutationByProduct(Func<Product, Task> func, Product product);
        Task DbMutationById(Func<int, Task> func, int id);
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task DeleteProduct(int id);
        Task UpdateProduct(Product product);
        Task AddProduct(Product product);

        Task<IEnumerable<ProductBrand>> GetProductBrandsAsync();
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
    }
}