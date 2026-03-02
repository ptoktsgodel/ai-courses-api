using Ai.Courses.Api.Models;
using Ai.Courses.Logic.Commands.AddItemPayment;
using Ai.Courses.Logic.Commands.DeleteItem;
using Ai.Courses.Logic.Commands.DeletePayment;
using Ai.Courses.Logic.Commands.UpdatePayment;
using Ai.Courses.Logic.Queries.GetItemById;
using Ai.Courses.Logic.Queries.GetItems;
using Ai.Courses.Logic.Queries.GetTypes;
using Asp.Versioning;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace Ai.Courses.Api.Endpoints.Items;

public static class ItemEndpoints
{
    public static WebApplication MapItemEndpoints(this WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("/api/v{version:apiVersion}/items")
            .WithApiVersionSet(versionSet)
            .WithTags("Items")
            .RequireAuthorization();

        group.MapGet("/types", async (
            HttpContext httpContext,
            ISender mediator) =>
        {
            var userId = GetUserId(httpContext);
            var result = await mediator.Send(new GetTypesQuery { UserId = userId });
            return Results.Ok(result);
        })
        .WithName("GetTypes")
        .WithSummary("Get all payment types for the current user")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/", async (
            DateTime dateFrom,
            DateTime dateTo,
            HttpContext httpContext,
            ISender mediator) =>
        {
            var userId = GetUserId(httpContext);
            var result = await mediator.Send(new GetItemsQuery { UserId = userId, DateFrom = dateFrom, DateTo = dateTo });
            return Results.Ok(result);
        })
        .WithName("GetItems")
        .WithSummary("Get all items with payments for a date range")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapGet("/{id:guid}", async (
            Guid id,
            HttpContext httpContext,
            ISender mediator) =>
        {
            var userId = GetUserId(httpContext);
            var result = await mediator.Send(new GetItemByIdQuery { Id = id, UserId = userId });

            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetItemById")
        .WithSummary("Get a single item with its payments")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (
            AddItemPaymentRequest request,
            HttpContext httpContext,
            ISender mediator,
            IValidator<AddItemPaymentCommand> validator) =>
        {
            var userId = GetUserId(httpContext);
            var command = new AddItemPaymentCommand
            {
                Date = request.Date,
                TypeName = request.TypeName,
                PlannedAmount = request.PlannedAmount,
                SpentAmount = request.SpentAmount,
                UserId = userId
            };

            var validation = await validator.ValidateAsync(command);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage }));

            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("AddItemPayment")
        .WithSummary("Add a payment to an item (creates item for the date if it doesn't exist)")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        group.MapPut("/{id:guid}/payments/{paymentId:guid}", async (
            Guid id,
            Guid paymentId,
            UpdatePaymentRequest request,
            HttpContext httpContext,
            ISender mediator,
            IValidator<UpdatePaymentCommand> validator) =>
        {
            var userId = GetUserId(httpContext);
            var command = new UpdatePaymentCommand
            {
                PaymentId = paymentId,
                ItemId = id,
                TypeName = request.TypeName,
                PlannedAmount = request.PlannedAmount,
                SpentAmount = request.SpentAmount,
                UserId = userId
            };

            var validation = await validator.ValidateAsync(command);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors
                    .Select(e => new { e.PropertyName, e.ErrorMessage }));

            var result = await mediator.Send(command);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("UpdatePayment")
        .WithSummary("Update a specific payment on an item")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}/payments/{paymentId:guid}", async (
            Guid id,
            Guid paymentId,
            HttpContext httpContext,
            ISender mediator) =>
        {
            var userId = GetUserId(httpContext);
            var deleted = await mediator.Send(new DeletePaymentCommand
            {
                PaymentId = paymentId,
                ItemId = id,
                UserId = userId
            });

            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeletePayment")
        .WithSummary("Delete a payment (also removes the item if it was the last payment)")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", async (
            Guid id,
            HttpContext httpContext,
            ISender mediator) =>
        {
            var userId = GetUserId(httpContext);
            var deleted = await mediator.Send(new DeleteItemCommand { ItemId = id, UserId = userId });

            return deleted ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteItem")
        .WithSummary("Delete an item and all its payments")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }

    private static Guid GetUserId(HttpContext httpContext)
        => Guid.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
