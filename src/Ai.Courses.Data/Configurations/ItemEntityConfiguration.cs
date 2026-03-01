using Ai.Courses.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ai.Courses.Data.Configurations;

public class ItemEntityConfiguration : IEntityTypeConfiguration<ItemEntity>
{
    public void Configure(EntityTypeBuilder<ItemEntity> builder)
    {
        builder.ToTable("Items");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.UserId).IsRequired();

        builder.Property(i => i.Date).IsRequired();

        builder.HasIndex(i => new { i.UserId, i.Date }).IsUnique();

        builder.HasMany(i => i.Payments)
            .WithOne(p => p.Item)
            .HasForeignKey(p => p.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
