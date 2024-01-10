using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecificationParameter SpecParams)
        : base(P =>
            (string.IsNullOrEmpty(SpecParams.Search) || P.Name.ToLower().Contains(SpecParams.Search)) &&
            (!SpecParams.brandID.HasValue || P.ProductBrandID == SpecParams.brandID.Value) &&
            (!SpecParams.typeID.HasValue || P.ProductTypeID == SpecParams.typeID.Value)
        )
        {

        }



    }
}
