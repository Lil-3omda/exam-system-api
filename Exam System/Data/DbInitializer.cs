using Exam_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Exam_System.Data
{
    public class DbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = {"Admin", "Student"};

            foreach (var roleName in roleNames)
            {
                if(!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            // 2. Create default admin user
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string adminEmail = "admin@gmail.com";
            string adminPassword = "Admin@123"; 
            string adminUsername = "admin";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminUsername,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    // Optional: log errors
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Admin user error: {error.Description}");
                    }
                }
            }


        }
    }
}
