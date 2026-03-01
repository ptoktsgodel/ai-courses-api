using Ai.Courses.Data;
using Ai.Courses.Data.Contexts;
using Ai.Courses.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ai.Courses.Api.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthConfiguration(this IServiceCollection services)
    {
        services
            .AddIdentityCore<UserEntity>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager();

        services
            .AddAuthentication(IdentityConstants.BearerScheme)
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorization();

        return services;
    }

    public static WebApplication UseAuthConfiguration(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}
