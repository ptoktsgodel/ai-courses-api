using Ai.Courses.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ai.Courses.Migrations.Seeders;

public static class DefaultAdminSeeder
{
    private const string AdminEmail = "admin@test.com";
    private const string AdminPassword = "Admin12345!";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<UserEntity>>();
        var logger = services.GetRequiredService<ILogger<UserEntity>>();

        if (await userManager.FindByEmailAsync(AdminEmail) is not null)
        {
            logger.LogInformation("Default admin user already exists. Skipping seed.");
            return;
        }

        var admin = new UserEntity
        {
            UserName = AdminEmail,
            Email = AdminEmail,
            FirstName = "Default",
            LastName = "Admin",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, AdminPassword);

        if (result.Succeeded)
            logger.LogInformation("Default admin user seeded successfully.");
        else
            logger.LogError("Failed to seed default admin user: {Errors}",
                string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
