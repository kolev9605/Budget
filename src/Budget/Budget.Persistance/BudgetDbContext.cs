using Budget.Core.Entities;
using Budget.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Budget.Persistance
{
    public class BudgetDbContext : IdentityDbContext<IdentityUser>
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
        }
    }
}
