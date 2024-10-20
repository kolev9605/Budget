using Budget.Domain.Constants;
using Budget.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Persistance.Configurations;

public class RecordConfiguration : IEntityTypeConfiguration<Record>
{
    public void Configure(EntityTypeBuilder<Record> builder)
    {
        builder.Property(p => p.Note)
            .HasMaxLength(Validations.Records.NoteMaxLength);

        builder.HasOne(r => r.PaymentType)
            .WithMany(pt => pt.Records)
            .HasForeignKey(r => r.PaymentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.CreatedOn)
            .HasDefaultValueSql("timezone('utc', now())");

        builder.Property(c => c.UpdatedOn)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
