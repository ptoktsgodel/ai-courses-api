using Ai.Courses.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ai.Courses.Data.Configurations;

public class PaymentEntityConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PlannedAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        builder.Property(p => p.SpentAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false);

        builder.HasOne(p => p.Type)
            .WithMany()
            .HasForeignKey(p => p.TypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
