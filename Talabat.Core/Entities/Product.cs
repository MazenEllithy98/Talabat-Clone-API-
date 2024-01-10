using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public decimal Price { get; set; }
        
        //[ForeignKey("ProductBrand")]
        public int ProductBrandID { get; set; } //Foreign Key , Doesn't Allow Nulls
        
        //[ForeignKey("ProductType")]
        public int ProductTypeID { get; set; } //ForeignKey

        public ProductBrand productBrand { get; set; } //Navigational Property - One
    
        public ProductType productType { get; set; } ////Navigational Property - One


    }
}
