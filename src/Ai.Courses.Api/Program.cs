using Ai.Courses.Api.Endpoints.Auth;
using Ai.Courses.Api.Endpoints.Items;
using Ai.Courses.Api.Extensions;
using Ai.Courses.Data;
using Ai.Courses.Data.Contexts;
using Ai.Courses.Logic.Commands.Login;
using Ai.Courses.Logic.Mappings;
using Ai.Courses.Logic.Validators;
using Ai.Courses.Migrations;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Data — registers UserDbContext and PaymentDbContext
builder.Services.AddDataConfiguration(builder.Configuration);

// Migrations — registers DbContextUser and DbContextPayment with MigrationsAssembly
builder.Services.AddMigrationsConfiguration(builder.Configuration);

// Auth (Identity + Bearer token)
builder.Services.AddAuthConfiguration();

// API Versioning
builder.Services.AddVersioningConfiguration();

// Swagger / OpenAPI
builder.Services.AddSwaggerConfiguration();

// CQRS - MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));

// Mapping - AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(UserProfile).Assembly));

// Validation - FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

var app = builder.Build();

// Swagger UI
app.UseSwaggerConfiguration();

// Auth middleware
app.UseAuthConfiguration();

// Run EF Core migrations and seed data (Development only)
await app.RunMigrationsAsync();

// Endpoints
app.MapAuthEndpoints();
app.MapItemEndpoints();

app.Run();
