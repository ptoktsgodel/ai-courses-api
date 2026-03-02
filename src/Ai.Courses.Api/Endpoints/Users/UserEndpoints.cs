using Ai.Courses.Logic.Models;
using Ai.Courses.Logic.Queries.GetUserById;
using Asp.Versioning;
using MediatR;
using System.Security.Claims;

namespace Ai.Courses.Api.Endpoints.Users;

public static class UserEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("/api/v{version:apiVersion}/users")
            .WithApiVersionSet(versionSet)
            .WithTags("Users")
            .RequireAuthorization();

        group.MapGet("/me", async (
            ClaimsPrincipal user,
            ISender mediator) =>
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id is null)
                return Results.Unauthorized();

            var result = await mediator.Send(new GetUserByIdQuery { Id = id });
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetCurrentUser")
        .WithSummary("Get the currently authenticated user")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{id}", async (
            string id,
            ClaimsPrincipal user,
            ISender mediator) =>
        {
            var requesterId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (requesterId != id)
                return Results.Forbid();

            var result = await mediator.Send(new GetUserByIdQuery { Id = id });
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetUserById")
        .WithSummary("Get user by ID")
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status403Forbidden)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
