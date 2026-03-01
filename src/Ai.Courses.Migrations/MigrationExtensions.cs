using Ai.Courses.Migrations.Contexts;
using Ai.Courses.Migrations.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ai.Courses.Migrations;

public static class MigrationExtensions
{
    /// <summary>
    /// Applies pending EF Core migrations and seeds default data.
    /// Runs only in the Development environment.
    /// </summary>
    public static async Task RunMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbContextUser>>();

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<DbContextUser>();
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");

            await DefaultAdminSeeder.SeedAsync(scope.ServiceProvider);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while running migrations or seeding data.");
            throw;
        }
    }
}
