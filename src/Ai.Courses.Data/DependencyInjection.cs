using Ai.Courses.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ai.Courses.Data;

public static class DependencyInjection
{
    /// <summary>
    /// Registers UserDbContext for application use (Identity, repositories).
    /// </summary>
    public static IServiceCollection AddDataConfiguration(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}
