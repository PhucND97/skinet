using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepo<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetBySpecification(ISpecification<T> spec);
        Task<IEnumerable<T>> GetAllBySpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        void Delete(T entity);
        void Add(T entity);
        void Update(T entity);
    }
}