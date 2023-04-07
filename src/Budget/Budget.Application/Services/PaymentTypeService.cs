using Budget.Application.Interfaces;
using Budget.Application.Interfaces.Services;
using Budget.Application.Models.PaymentTypes;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly IBudgetDbContext _budgetDbContext;

        public PaymentTypeService(IBudgetDbContext budgetDbContext)
        {
            _budgetDbContext = budgetDbContext;
        }

        public async Task<IEnumerable<PaymentTypeModel>> GetAllAsync()
        {
            var paymentTypes = await _budgetDbContext.PaymentTypes
                .ProjectToType<PaymentTypeModel>()
                .ToListAsync();

            return paymentTypes;
        }
    }
}
