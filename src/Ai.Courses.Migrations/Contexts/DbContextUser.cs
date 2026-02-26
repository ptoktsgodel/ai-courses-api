using Ai.Courses.Data;
using Microsoft.EntityFrameworkCore;

namespace Ai.Courses.Migrations.Contexts;

/// <summary>
/// Migration-level context for the User domain.
/// Inherits UserDbContext and is used exclusively by dotnet ef tooling.
/// Additional domains should introduce their own DbContextXxx subclass here.
/// </summary>
public class DbContextUser : UserDbContext
{
    public DbContextUser(DbContextOptions<DbContextUser> options) : base(options) { }
}
