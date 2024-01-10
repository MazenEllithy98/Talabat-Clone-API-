using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
    public class BasketItemDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PictureURL { get; set; }
        [Required]
        [Range(0.1, double.MaxValue , ErrorMessage = "Price Must Be Greater than Zero!")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue , ErrorMessage ="Quantity Must be Atleast One !")]
        public int Quantity { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
    }
}