# Copilot Instructions — ai-courses-api

## Architecture Overview

Four-project solution following a strict layered separation:

| Project | Role |
|---|---|
| `Ai.Courses.Api` | Minimal API endpoints, DI wiring, extension methods |
| `Ai.Courses.Logic` | CQRS commands/handlers, FluentValidation validators, AutoMapper profiles |
| `Ai.Courses.Data` | EF Core `DbContext`, `IdentityUser` entity, EF configurations |
| `Ai.Courses.Migrations` | EF migrations, `DbContextUser` (migration-only subcontext), seeders |

Dependency direction: `Api` → `Logic` → `Data`; `Api` → `Migrations` → `Data`.

## Key Patterns

### CQRS via MediatR
Every feature is a command or query in `Ai.Courses.Logic/Commands/<Feature>/`:
- `XxxCommand.cs` — implements `IRequest<TResponse>`
- `XxxCommandHandler.cs` — implements `IRequestHandler<XxxCommand, TResponse>`

Handlers are registered in `Program.cs` via `AddMediatR` scanning `LoginCommandHandler`'s assembly.

### Validation
`FluentValidation` validators live in `Ai.Courses.Logic/Validators/`. Validation is invoked **manually** inside endpoint handlers — not via pipeline behavior:
```csharp
var validation = await validator.ValidateAsync(command);
if (!validation.IsValid)
    return Results.BadRequest(validation.Errors
        .Select(e => new { e.PropertyName, e.ErrorMessage }));
```

### Minimal API Endpoints
Endpoints are grouped in static `Map*Endpoints()` extension methods under `Ai.Courses.Api/Endpoints/<Domain>/`. Route template: `/api/v{version:apiVersion}/<domain>`. Each group declares an `ApiVersionSet` and uses `.WithTags`, `.WithName`, `.Produces` for Swagger.

### Auth
ASP.NET Core Identity + built-in Bearer token middleware (`IdentityConstants.BearerScheme`). No JWT configuration — login returns `Results.SignIn(principal, IdentityConstants.BearerScheme)`.

### Migrations Split Pattern
`UserDbContext` (in `Ai.Courses.Data`) is the app-level context. `DbContextUser` (in `Ai.Courses.Migrations`) is a **migration-only subclass** registered separately so EF tooling can locate the migrations assembly. Both contexts are registered in DI; only `DbContextUser` is used by `MigrationExtensions.RunMigrationsAsync()`.

For a new domain, add a `DbContextXxx : XxxDbContext` to `Ai.Courses.Migrations/Contexts/`.

## Developer Workflows

**Build:**
```
dotnet build
```

**Run (Development):**
```
dotnet run --project src/Ai.Courses.Api
```
On first run, `RunMigrationsAsync()` auto-applies migrations and seeds `admin@ai-courses.com` / `Admin@123456`.

**Add a migration:**
```
dotnet ef migrations add <Name> --project src/Ai.Courses.Migrations --startup-project src/Ai.Courses.Api --context DbContextUser
```

**Database:** SQLite (`ai-courses.db`, created at app root). Connection string is in `appsettings.json` → `ConnectionStrings:DefaultConnection`.

## Adding a New Feature Checklist

1. Add `XxxCommand` + `XxxCommandHandler` in `Ai.Courses.Logic/Commands/<Feature>/`
2. Add a `FluentValidation` validator in `Ai.Courses.Logic/Validators/`
3. Add or extend an `AutoMapper` profile in `Ai.Courses.Logic/Mappings/` if mapping entities to DTOs
4. Add endpoint mapping in `Ai.Courses.Api/Endpoints/<Domain>/`, registering with `app.Map*Endpoints()` in `Program.cs`
5. If new entities are needed, add to `Ai.Courses.Data/Entities/`, configure in `Ai.Courses.Data/Configurations/`, and run an EF migration using the command above

## Key Files

- [src/Ai.Courses.Api/Program.cs](src/Ai.Courses.Api/Program.cs) — DI registration and middleware pipeline
- [src/Ai.Courses.Api/Endpoints/Auth/AuthEndpoints.cs](src/Ai.Courses.Api/Endpoints/Auth/AuthEndpoints.cs) — canonical endpoint pattern
- [src/Ai.Courses.Logic/Commands/Login/LoginCommandHandler.cs](src/Ai.Courses.Logic/Commands/Login/LoginCommandHandler.cs) — canonical handler pattern
- [src/Ai.Courses.Migrations/Contexts/DbContextUser.cs](src/Ai.Courses.Migrations/Contexts/DbContextUser.cs) — migration split pattern
- [src/Ai.Courses.Migrations/MigrationExtensions.cs](src/Ai.Courses.Migrations/MigrationExtensions.cs) — auto-migration on startup (Development only)
