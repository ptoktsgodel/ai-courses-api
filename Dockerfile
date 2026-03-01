# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files for layer-cached restore
COPY ai-courses-api.sln ./
COPY src/Ai.Courses.Api/Ai.Courses.Api.csproj                   src/Ai.Courses.Api/
COPY src/Ai.Courses.Logic/Ai.Courses.Logic.csproj               src/Ai.Courses.Logic/
COPY src/Ai.Courses.Data/Ai.Courses.Data.csproj                 src/Ai.Courses.Data/
COPY src/Ai.Courses.Migrations/Ai.Courses.Migrations.csproj     src/Ai.Courses.Migrations/

RUN dotnet restore

# Copy full source and publish
COPY . .
RUN dotnet publish src/Ai.Courses.Api/Ai.Courses.Api.csproj \
    -c Release \
    --no-restore \
    -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Ai.Courses.Api.dll"]
