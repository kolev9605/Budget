using Budget.Core.Entities;
using Budget.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Budget.Persistance
{
    public class BudgetDbContext : DbContext, IBudgetDbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options)
            : base(options)
        {

        }

        public DbSet<Record> Records { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BudgetDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
