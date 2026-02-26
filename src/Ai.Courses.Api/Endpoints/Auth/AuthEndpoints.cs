using Ai.Courses.Logic.Commands.Login;
using Ai.Courses.Logic.Commands.Register;
using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ai.Courses.Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("/api/v{version:apiVersion}/auth")
            .WithApiVersionSet(versionSet)
            .WithTags("Auth");

        group.MapPost("/register", async (
            RegisterCommand command,
            ISender mediator,
            IValidator<RegisterCommand> validator) =>
        {
            var validation = await validator.ValidateAsync(command);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage }));

            var result = await mediator.Send(command);

            if (result is null)
                return Results.BadRequest("Registration failed.");

            return Results.Created("/api/v1/auth", result);
        })
        .WithName("Register")
        .WithSummary("Register a new user")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/login", async (
            LoginCommand command,
            ISender mediator,
            IValidator<LoginCommand> validator) =>
        {
            var validation = await validator.ValidateAsync(command);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage }));

            var principal = await mediator.Send(command);
            if (principal is null)
                return Results.Unauthorized();

            return Results.SignIn(principal, authenticationScheme: IdentityConstants.BearerScheme);
        })
        .WithName("Login")
        .WithSummary("Login and receive a Bearer token")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}
