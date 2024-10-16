using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Persistance.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsUnicode(true)
            .IsRequired();

        builder.HasMany(c => c.Records)
            .WithOne(r => r.Category)
            .HasForeignKey(r => r.CategoryId);

        builder.HasMany(pc => pc.SubCategories)
            .WithOne(sc => sc.ParentCategory)
            .HasForeignKey(sc => sc.ParentCategoryId);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("timezone('utc', now())");

        builder.Property(c => c.UpdatedAt)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
