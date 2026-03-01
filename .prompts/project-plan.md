
Build a production-ready boilerplate for a RESTful Web API serving an AI courses platform using .NET 9 and C# 12+.

The solution must follow Clean Architecture with strict layer separation, CQRS via MediatR, and ASP.NET Core Identity.

## Solution Structure

Create a four-project solution under a `src/` folder:

- `Ai.Courses.Api` — Minimal API endpoints, DI wiring, extension methods
- `Ai.Courses.Logic` — CQRS commands/handlers, FluentValidation validators, AutoMapper profiles
- `Ai.Courses.Data` — EF Core `DbContext`, Identity entity, EF Fluent configurations, generic repository
- `Ai.Courses.Migrations` — EF migrations, design-time factory, seeders

Dependency direction: `Api` → `Logic` → `Data`; `Api` → `Migrations` → `Data`.

## Requirements

- **Authentication** — ASP.NET Core Identity with built-in Bearer token (`IdentityConstants.BearerScheme`). No JWT configuration. Login returns `Results.SignIn(principal, IdentityConstants.BearerScheme)`.
- **CQRS** — Every feature is a command or query in `Ai.Courses.Logic/Commands/<Feature>/`. Each feature folder contains `XxxCommand.cs` (implements `IRequest<TResponse>`) and `XxxCommandHandler.cs` (implements `IRequestHandler`).
- **Validation** — FluentValidation validators in `Ai.Courses.Logic/Validators/`. Invoked manually inside endpoint handlers — no pipeline behavior.
- **Mapping** — AutoMapper profiles co-located in `Ai.Courses.Logic/Mappings/`. Entities must never cross the Logic boundary; use DTOs.
- **Database** — EF Core with SQLite. Apply EF configurations explicitly via Fluent API (no assembly scanning). Use `AsNoTracking` where appropriate.
- **Migrations split** — `UserDbContext` in `Ai.Courses.Data` is the app-level context. `DbContextUser : UserDbContext` in `Ai.Courses.Migrations` is a migration-only subclass used by EF tooling and `MigrationExtensions`. A generic `DbContextFactory<TContext>` reads the connection string from `appsettings.json` at design time.
- **Auto-migration** — `MigrationExtensions.RunMigrationsAsync()` applies pending migrations and seeds default data on startup. Runs only in the Development environment.
- **Seed data** — Seed a default admin user (`admin@ai-courses.com` / `Admin@123456`) on first run via `DefaultAdminSeeder`.
- **API style** — Minimal API with URL-segment versioning (`/api/v{version:apiVersion}/...`). Group endpoints in static `Map*Endpoints()` extension methods under `Ai.Courses.Api/Endpoints/<Domain>/`.
- **Documentation** — Swagger UI configured with a Bearer security scheme. Each endpoint declares `.WithTags`, `.WithName`, and `.Produces` metadata.
- **Repository** — Generic `IRepository<T>` / `Repository<T>` in `Ai.Courses.Data`.

## API Endpoints

| Method | Route |
|--------|-------|
| POST | `/api/v1/auth/register` |
| POST | `/api/v1/auth/login` |
