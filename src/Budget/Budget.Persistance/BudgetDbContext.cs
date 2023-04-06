using Budget.Core.Entities;
using Budget.Application.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Budget.Persistance
{
    public class BudgetDbContext : IdentityDbContext<ApplicationUser>, IBudgetDbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
            : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Currency> Currencies { get; set; }
        
        public DbSet<PaymentType> PaymentTypes { get; set; }
        
        public DbSet<Record> Records { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<UserCategory> UserCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
