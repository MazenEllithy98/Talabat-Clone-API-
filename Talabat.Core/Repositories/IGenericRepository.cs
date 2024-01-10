using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Specs);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> Specs);
    
        Task<int> GetCountWithSpecAsync(ISpecification<T> Specs);

        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    
    }
}
