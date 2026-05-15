using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
namespace EcommerceApplication.Data.Seeder
{

    public static class RoleSeeder
    {
        public static async Task Seed(IServiceProvider serviceProvider)//serviceProvider gives access to the app’s Dependency Injection container
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "ADMIN", "CUSTOMER" };

            foreach (var role in roles)//adding each role into database if it doesn't exist
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}