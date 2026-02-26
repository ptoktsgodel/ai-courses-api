# AI Courses API — Project Plan

## Goal

Build a production-ready boilerplate for a RESTful Web API serving an AI courses platform.
The solution follows Clean Architecture with strict layer separation, CQRS, and ASP.NET Core Identity.

---

## Requirements

- **Authentication** — ASP.NET Core Identity with built-in Bearer token (no JWT configuration)
- **Architecture** — Clean Architecture, CQRS via MediatR, Repository pattern
- **Validation** — FluentValidation, invoked manually in endpoints (no pipeline behavior)
- **Mapping** — AutoMapper with co-located profiles
- **Database** — SQLite via EF Core, auto-migrated on startup in Development
- **API style** — Minimal API with URL-segment versioning (`/api/v1/...`)
- **Documentation** — Swagger UI with Bearer security scheme
- **Seed data** — Default admin user created on first run

---

## Solution Structure

```
ai-courses-api/
├── src/
│   ├── Ai.Courses.Api          # Minimal API endpoints, DI wiring, extension methods
│   ├── Ai.Courses.Logic        # CQRS commands/handlers, validators, AutoMapper profiles
│   ├── Ai.Courses.Data         # EF Core DbContext, Identity entity, EF configurations, repository
│   └── Ai.Courses.Migrations   # EF migrations, design-time factory, seeders
├── tests/                      # Reserved for future tests
└── .github/
    ├── copilot-instructions.md
    └── instructions/
```

---

## Layer Responsibilities

### Api
- Compose the application (DI, middleware pipeline)
- Define Minimal API endpoint groups with versioning, Swagger metadata, and validation calls
- No business logic — thin delegation to MediatR

### Logic
- Handlers call `UserManager` / domain services; never expose entities across the boundary
- DTOs (`UserDto`) are the only types that cross into Api
- Validators live in `Validators/`, profiles in `Mappings/`

### Data
- `UserDbContext` inherits `IdentityDbContext<UserEntity>`
- Generic `IRepository<T>`
- EF configurations applied explicitly (no assembly scanning)

### Migrations
- `DbContextUser` inherits `UserDbContext` — for EF tooling only
- Generic design-time factory reads connection string from `appsettings.json`
- `MigrationExtensions.RunMigrationsAsync()` auto-applies migrations and seeds on startup (Development only)

---

## API Endpoints

| Method | Route |
|--------|-------|
| POST | `/api/v1/auth/register`
| POST | `/api/v1/auth/login`

---

## Key Technical Decisions

| Decision | Choice | Reason |
|----------|--------|--------|
| Token type | Identity BearerToken (built-in) | No JWT config overhead for a boilerplate |
| Migrations project type | Class Library | No entry point needed; EF tooling uses design-time factory |
