using DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace DAL.Init
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
        {
            string adminEmail = configuration.GetValue<string>("AdminData:AdminEmail");
            string password = configuration.GetValue<string>("AdminData:AdminPassword");

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("admin"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new()
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin",
                    Surname = "Admin",
                    EmailConfirmed = true,
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("user"));
            }
        }
    }
}
