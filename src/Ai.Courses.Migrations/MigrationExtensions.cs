using Ai.Courses.Migrations.Contexts;
using Ai.Courses.Migrations.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ai.Courses.Migrations;

public static class MigrationExtensions
{
    public static async Task RunMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbContextUser>>();

        try
        {
            var userContext = scope.ServiceProvider.GetRequiredService<DbContextUser>();
            await userContext.Database.MigrateAsync();
            logger.LogInformation("User database migrations applied successfully.");

            await DefaultAdminSeeder.SeedAsync(scope.ServiceProvider);

            var paymentsContext = scope.ServiceProvider.GetRequiredService<DbContextPayment>();
            await paymentsContext.Database.MigrateAsync();
            logger.LogInformation("Payments database migrations applied successfully.");

            await PaymentsSeeder.SeedAsync(scope.ServiceProvider);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while running migrations or seeding data.");
            throw;
        }
    }
}
