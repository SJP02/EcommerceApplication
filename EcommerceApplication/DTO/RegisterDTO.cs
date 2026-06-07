using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.DTO
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}