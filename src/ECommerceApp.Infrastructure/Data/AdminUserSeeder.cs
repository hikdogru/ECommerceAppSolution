using ECommerceApp.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Infrastructure.Data;

public class AdminUserSeeder
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public AdminUserSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAdminUserAsync()
    {
        // Check if the admin role exists; create it if not
        if (!_roleManager.Roles.Any(r => r.Name == "Admin"))
        {
            await _roleManager.CreateAsync(new AppRole()
            {
                Name = "Admin"
            });
        }

        // Check if the admin user exists; create it if not
        if (_userManager.Users.All(u => u.UserName != "admin@admin.com"))
        {
            var adminUser = new AppUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin123.");

            if (result.Succeeded)
            {
                // Assign the Admin role to the admin user
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
