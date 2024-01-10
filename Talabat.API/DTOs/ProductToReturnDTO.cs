using Talabat.Core.Entities;

namespace Talabat.API.DTOs
{
    public class ProductToReturnDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public decimal Price { get; set; }

        //[ForeignKey("ProductBrand")]
        public int ProductBrandID { get; set; } 

        //[ForeignKey("ProductType")]
        public int ProductTypeID { get; set; } 

        public string ProductBrand { get; set; }

        public string ProductType { get; set; } 
    }
}
