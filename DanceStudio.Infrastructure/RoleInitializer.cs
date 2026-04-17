using Microsoft.AspNetCore.Identity;
using DanceStudio.Domain.Model;

namespace DanceStudio.Infrastructure;

public static class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "Coach", "Client" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        string adminEmail = "admin@dance.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new AppUser 
            { 
                UserName = adminEmail, 
                Email = adminEmail 
            };
            await userManager.CreateAsync(admin, "Admin_123");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
