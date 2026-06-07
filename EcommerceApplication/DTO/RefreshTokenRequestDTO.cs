using System.ComponentModel.DataAnnotations;

namespace EcommerceApplication.DTO
{
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string Token {  get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}