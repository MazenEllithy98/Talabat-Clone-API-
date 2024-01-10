using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        
        [Required]
        public string Password { get; set; }
       
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    
    }
}
