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

            modelBuilder.Entity<Currency>().HasData(
                new Currency() { Id = 1, Name = "BGN" },
                new Currency() { Id = 2, Name = "USD" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
