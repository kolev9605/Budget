using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Budget.Persistance.Configurations
{
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
        }
    }
}
