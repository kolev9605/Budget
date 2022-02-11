using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Persistance.Seeders
{
    public static class Seeder
    {
        public static void Seed(this IApplicationBuilder app, bool development = false)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<BudgetDbContext>())
            {
                context.Migrate();
                context.Seed();
            }
        }

        private static void Migrate(this BudgetDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();
        }

        private static void Seed(this BudgetDbContext context)
        {
            context
                .AddRoles()
                .AddCurrencies();

            context.SaveChanges();
        }
    }
}
