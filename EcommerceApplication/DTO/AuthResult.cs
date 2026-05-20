using EcommerceApplication.Models;

namespace EcommerceApplication.DTO
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public ApplicationUser User { get; set; }
    }
}