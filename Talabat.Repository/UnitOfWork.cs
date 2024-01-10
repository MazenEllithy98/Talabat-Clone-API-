using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _Repositories;


        public UnitOfWork(StoreContext dbContext )
        {
            _dbContext = dbContext;
           
        }

        public IGenericRepository<TEntity>? Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_Repositories == null )
                _Repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_Repositories.ContainsKey( type ) )
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _Repositories.Add( type, repository );

            }

            return _Repositories[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
        => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        => await _dbContext.DisposeAsync();

       
    }
}
