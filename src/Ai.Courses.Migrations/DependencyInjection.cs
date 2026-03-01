using Ai.Courses.Migrations.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ai.Courses.Migrations;

public static class DependencyInjection
{
    /// <summary>
    /// Registers DbContextUser with MigrationsAssembly for runtime migration execution.
    /// DbContextUser is the migration-level context — only used by MigrationExtensions.
    /// </summary>
    public static IServiceCollection AddMigrationsConfiguration(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<DbContextUser>(options =>
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly("Ai.Courses.Migrations")));

        return services;
    }
}
