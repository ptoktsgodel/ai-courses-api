using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ai.Courses.Migrations.Factories;

/// <summary>
/// Generic design-time factory base. Subclass for each DbContext domain.
/// To add a new domain: create DbContextXxx : TargetDbContext and XxxFactory : DbContextFactory&lt;DbContextXxx&gt;.
/// </summary>
public abstract class DbContextFactory<TContext> : IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
    public TContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");

        var optionsBuilder = new DbContextOptionsBuilder<TContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return CreateContext(optionsBuilder.Options);
    }

    protected abstract TContext CreateContext(DbContextOptions<TContext> options);
}
