using Budget.Core.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Budget.Persistance.Seeders
{
    public static class RolesSeeder
    {
        public static async Task<BudgetDbContext> AddRolesAsync(this BudgetDbContext context, RoleManager<IdentityRole> roleManager)
        {            
            if (!await roleManager.RoleExistsAsync(Roles.Administrator))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Administrator));
            }

            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
            }

            return context;
        }
    }
}
