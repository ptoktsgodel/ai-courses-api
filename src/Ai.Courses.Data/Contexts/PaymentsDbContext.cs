using Ai.Courses.Data.Configurations;
using Ai.Courses.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ai.Courses.Data.Contexts;

public class PaymentDbContext : DbContext
{
    public DbSet<ItemEntity> Items { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }
    public DbSet<TypeEntity> Types { get; set; }

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    protected PaymentDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ItemEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TypeEntityConfiguration());
    }
}
