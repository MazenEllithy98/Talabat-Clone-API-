using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext DbContext) //ask CLR to create object from DbContext Implicitly
        {
            _dbContext = DbContext;
        }

        public async Task Add(T entity)
        => await _dbContext.Set<T>().AddAsync(entity);

        public void Delete(T entity)
        => _dbContext.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>) await _dbContext.Products.Include(P => P.productBrand).Include(P => P.productType).ToListAsync();
            }
            else
            {
                return await _dbContext.Set<T>().ToListAsync();

            }
        }
        
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Specs)
        {
            return await ApplySpecification(Specs).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //return await _dbContext.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> Specs)
        {
            return await ApplySpecification(Specs).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> Specs)
        {
            return await ApplySpecification(Specs).CountAsync();
        }

        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);

        private IQueryable<T> ApplySpecification (ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

    }
}
