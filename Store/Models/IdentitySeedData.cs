using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Store.Models;

public static class IdentitySeedData
{
    private const string AdminUser = "Admin";
    private const string AdminPassword = "Secret123$";

    public static async void EnsurePopulated(IApplicationBuilder app)
    {
        AppIdentityDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<AppIdentityDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        UserManager<IdentityUser> userManager = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();
        IdentityUser? user = await userManager.FindByNameAsync(AdminUser);
        if (user == null)
        {
            user = new IdentityUser("Admin");
            user.Email = "admin@example.com";
            user.PhoneNumber = "555-1234";
            await userManager.CreateAsync(user, AdminPassword);
        }
    }
}