using Ai.Courses.Api.Endpoints.Auth;
using Ai.Courses.Api.Extensions;
using Ai.Courses.Data;
using Ai.Courses.Logic.Commands.Login;
using Ai.Courses.Logic.Mappings;
using Ai.Courses.Logic.Validators;
using Ai.Courses.Migrations;
using FluentValidation;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Data — registers UserDbContext for Identity and app usage
builder.Services.AddDataConfiguration(connectionString);

// Migrations — registers DbContextUser with MigrationsAssembly for runtime migration execution
builder.Services.AddMigrationsConfiguration(connectionString);

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
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<UserProfile>());

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

app.Run();
