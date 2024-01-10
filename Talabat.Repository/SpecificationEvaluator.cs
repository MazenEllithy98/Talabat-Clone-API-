using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecification<TEntity> Specs)
        {
            var query = inputQuery;
            if (Specs.Criteria is not null)
            {
                query = query.Where(Specs.Criteria);
            }

            if (Specs.OrderBy is not null)
                query = query.OrderBy(Specs.OrderBy);
            
            if (Specs.OrderByDescending is not null)
                query = query.OrderByDescending(Specs.OrderByDescending);
 
            if (Specs.PaginationEnable is true)
            {
                query = query.Skip(Specs.Skip).Take(Specs.Take);
            }                      
            ///Includes
            ///1.P=>P.ProductBrand
            ///2.P=>P.ProductType
            query = Specs.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            
            
            return query;
        }    
            

     }
}
