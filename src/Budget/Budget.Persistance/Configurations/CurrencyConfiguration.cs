using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Persistance.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.HasData(
                new Currency() { Id = 1, Name = "Bulgarian lev", Abbreviation = "BGN" },
                new Currency() { Id = 2, Name = "European Euro", Abbreviation = "EUR" },
                new Currency() { Id = 3, Name = "U.S. Dollar", Abbreviation = "USD" }
            );
        }
    }
}
