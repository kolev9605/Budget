using Budget.Core.Entities;
using Budget.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Budget.Persistance
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
            : base(options)
        {

        }

        public DbSet<Record> Records { get; set; }

        public DbSet<Currency> Currencies { get; set; }
        
        public DbSet<PaymentType> PaymentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetDbContext).Assembly);
            
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Currency>().HasData(
            //    new Currency() { Id = 1, Name = "Bulgarian lev", Abbreviation = "BGN" },
            //    new Currency() { Id = 2, Name = "European Euro", Abbreviation = "EUR" },
            //    new Currency() { Id = 3, Name = "U.S. Dollar", Abbreviation = "USD" }
            //);

            //modelBuilder.Entity<PaymentType>().HasData(
            //    new PaymentType() { Id = 1, Name = "Debit Card" },
            //    new PaymentType() { Id = 2, Name = "Cash" }
            //);

            //modelBuilder.Entity<Record>().HasData(
            //    new Record() { Id = 1, Amount = 20m, CurrencyId = 1, DateAdded = DateTime.UtcNow, Note = "test data", PaymentTypeId = 1 },
            //    new Record() { Id = 2, Amount = 15m, CurrencyId = 1, DateAdded = DateTime.UtcNow, Note = "test data 2", PaymentTypeId = 2 },
            //    new Record() { Id = 3, Amount = 25m, CurrencyId = 1, DateAdded = DateTime.UtcNow, Note = "test data 3", PaymentTypeId = 2 }
            //);

            
        }
    }
}
