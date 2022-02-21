using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Budget.Persistance.Seeders
{
    public static class Seeder
    {
        public static async Task SeedAsync(this IApplicationBuilder app, bool development = false)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<BudgetDbContext>())
            using (var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>())
            {
                await context.SeedAsync(roleManager);
            }
        }

        private static async Task SeedAsync(this BudgetDbContext context, RoleManager<IdentityRole> roleManager)
        {
            await context.AddRolesAsync(roleManager);
            await context.AddCurrenciesAsync();

            await context.SaveChangesAsync();
        }
    }
}
