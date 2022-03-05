using Budget.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Budget.Persistance.Seeders
{
    public static class PaymentTypesSeeder
    {
        public static async Task<BudgetDbContext> AddPaymentTypesAsync(this BudgetDbContext context)
        {
            if (!await context.PaymentTypes.AnyAsync())
            {
                context.Add(new PaymentType() { Name = "Cash"});
                context.Add(new PaymentType() { Name = "Debit Card"});
                context.Add(new PaymentType() { Name = "Credit Card"});
            }

            return context;
        }
    }
}
