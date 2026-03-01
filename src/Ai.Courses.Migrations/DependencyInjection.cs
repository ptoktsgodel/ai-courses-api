using Ai.Courses.Migrations.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ai.Courses.Migrations;

public static class DependencyInjection
{
    public static IServiceCollection AddMigrationsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<DbContextUser>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("UsersConnection"),
                b => b.MigrationsAssembly("Ai.Courses.Migrations")));

        services.AddDbContext<DbContextPayment>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("PaymentsConnection"),
                b => b.MigrationsAssembly("Ai.Courses.Migrations")));

        return services;
    }
}
