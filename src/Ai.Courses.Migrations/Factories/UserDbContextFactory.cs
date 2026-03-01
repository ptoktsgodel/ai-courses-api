using Ai.Courses.Migrations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Ai.Courses.Migrations.Factories;

public class UserDbContextFactory : DbContextFactory<DbContextUser>
{
    protected override string ConnectionStringKey => "UsersConnection";

    protected override DbContextUser CreateContext(DbContextOptions<DbContextUser> options)
        => new DbContextUser(options);
}
