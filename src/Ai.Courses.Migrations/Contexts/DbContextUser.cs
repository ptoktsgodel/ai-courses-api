using Ai.Courses.Data;
using Ai.Courses.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Ai.Courses.Migrations.Contexts;

public class DbContextUser : UserDbContext
{
    public DbContextUser(DbContextOptions<DbContextUser> options) : base(options) { }
}
