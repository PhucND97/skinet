using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
    {
        private readonly StoreContext _context;
        public GenericRepo(StoreContext context)
        {
            _context = context;
        }
        public async Task Add(T t)
        {
            await _context.Set<T>().AddAsync(t);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await Validate(id);
        }
        public async Task<IEnumerable<T>> GetAllBySpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetBySpecification(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task Delete(int id)
        {
            var toRemove = await Validate(id);
            await Task.Run(() => _context.Set<T>().Remove(toRemove));
        }
        public Task Update(T t)
        {
            throw new System.NotImplementedException();
        }

        public async Task<T> Validate(int id)
        {
            var item = _context.Set<T>().FirstOrDefaultAsync(i => i.Id == id);
            if (item is null)
            {
                throw new ArgumentException();
            }
            return await item;
        }

        public IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
    }
}