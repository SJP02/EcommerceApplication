using System;
using Microsoft.AspNetCore.Identity;
namespace EcommerceApplication.Models
{	
    public class ApplicationUser : IdentityUser
	{
	    public List<Order> Orders { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
