using Ai.Courses.Migrations.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Ai.Courses.Migrations.Factories;

public class PaymentDbContextFactory : DbContextFactory<DbContextPayment>
{
    protected override string ConnectionStringKey => "PaymentsConnection";

    protected override DbContextPayment CreateContext(DbContextOptions<DbContextPayment> options)
        => new DbContextPayment(options);
}
