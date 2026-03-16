Create a multi-container Docker setup for a .NET 9 Web API and a Microsoft SQL Server database.

Requirements:

- Provide a multi-stage `Dockerfile` for the Web API project that handles building and running the app.
- Provide a `docker-compose.yml` that orchestrates both the `webapi` and `sqlserver` services.
- Add a health check to the `sqlserver` service so the API container waits until SQL Server is ready before starting.
- The `webapi` service must depend on `sqlserver` using `condition: service_healthy`.
- Pass the database connection string to the Web API via an environment variable
- Add a named volume for SQL Server data so it persists across `docker compose down` restarts.
- Add a `.dockerignore` file to exclude build artifacts and unnecessary files from the Docker build context.
- Switch the EF Core provider from SQLite to SQL Server across all projects and regenerate migrations.
- Ensure migrations and seeding run on every startup regardless of environment.
