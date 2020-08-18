using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace API.Repositories
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
    }
}