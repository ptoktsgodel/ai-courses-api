using Ai.Courses.Data.Configurations;
using Ai.Courses.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ai.Courses.Data;

public class UserDbContext : IdentityDbContext<UserEntity>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    // Protected constructor allows subclasses (e.g. DbContextUser in Migrations) to pass typed options
    protected UserDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
    }
}
