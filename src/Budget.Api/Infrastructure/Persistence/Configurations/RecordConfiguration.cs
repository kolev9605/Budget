using Budget.Api.Domain.Constants;
using Budget.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Api.Infrastructure.Persistence.Configurations;

public class RecordConfiguration : IEntityTypeConfiguration<Record>
{
    public void Configure(EntityTypeBuilder<Record> builder)
    {
        builder.Property(p => p.Note)
            .HasMaxLength(Validations.Records.NoteMaxLength);

        builder.Property(c => c.CreatedOn)
            .HasDefaultValueSql("timezone('utc', now())");

        builder.Property(c => c.UpdatedOn)
            .HasDefaultValueSql("timezone('utc', now())");
    }
}
