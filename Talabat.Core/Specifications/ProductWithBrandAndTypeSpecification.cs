using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        //This Constructor is used for GetAllProducts
        public ProductWithBrandAndTypeSpecification(ProductSpecificationParameter productSpecsParams)
        : base(P =>
            (string.IsNullOrEmpty(productSpecsParams.Search) || P.Name.ToLower().Contains(productSpecsParams.Search)) &&
            (!productSpecsParams.brandID.HasValue || P.ProductBrandID == productSpecsParams.brandID.Value) &&
            (!productSpecsParams.typeID.HasValue || P.ProductTypeID == productSpecsParams.typeID.Value)

        )
        {
            Includes.Add(P => P.productBrand);
            Includes.Add(P => P.productType);

            AddOrderBy(P => P.Name);

            if (!string.IsNullOrEmpty(productSpecsParams.sort))
            {
                switch (productSpecsParams.sort)
                {
                   case "priceAsc" :
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc" :
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

            //Total Products = 100
            //PageSize = 10
            //PageIndex = 3

            ApplyPagination(productSpecsParams.PageSize * (productSpecsParams.PageIndex - 1), productSpecsParams.PageSize);


        }
        //This constructor is used for get a specific product by ID
        public ProductWithBrandAndTypeSpecification(int id) : base (P=> P.Id == id) //Criteria 
        {
            Includes.Add(P => P.productBrand);
            Includes.Add(P => P.productType);
        }
    }
}
