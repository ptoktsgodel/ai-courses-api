using Ai.Courses.Data;
using Ai.Courses.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Ai.Courses.Migrations.Contexts;

public class DbContextPayment : PaymentDbContext
{
    public DbContextPayment(DbContextOptions<DbContextPayment> options) : base(options) { }
}
