﻿using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Persistance.Configurations
{
    public class RecordConfiguration : IEntityTypeConfiguration<Record>
    {
        public void Configure(EntityTypeBuilder<Record> builder)
        {
            builder.Property(p => p.Note)
                .HasMaxLength(100);

            builder.HasOne(r => r.PaymentType)
                .WithMany(pt => pt.Records)
                .HasForeignKey(r => r.PaymentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
