using Budget.Core.Constants;
using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Persistance.Configurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(Validations.Accounts.NameMaxLength);

            builder.Property(a => a.UserId)
                .IsRequired();

            builder.HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Records)
                .WithOne(r => r.Account)
                .HasForeignKey(r => r.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.TransferRecords)
                .WithOne(r => r.FromAccount)
                .HasForeignKey(r => r.FromAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Currency)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
