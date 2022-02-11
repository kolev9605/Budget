using Budget.Core.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Budget.Persistance.Seeders
{
    public static class RolesSeeder
    {
        public static BudgetDbContext AddRoles(this BudgetDbContext context)
        {
            if (context.Roles.SingleOrDefault(r => r.Name == Roles.Administrator) == null)
            {
                context.Roles.Add(new IdentityRole(Roles.Administrator));
            }

            if (context.Roles.SingleOrDefault(r => r.Name == Roles.User) == null)
            {
                context.Roles.Add(new IdentityRole(Roles.User));
            }

            return context;
        }
    }
}
