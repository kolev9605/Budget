using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.PaymentTypes;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Budget.Persistance.Repositories;

public class PaymentTypeRepository : Repository<PaymentType>, IPaymentTypeRepository
{
    public PaymentTypeRepository(BudgetDbContext budgetDbContext) : base(budgetDbContext)
    {
    }

    public async Task<IEnumerable<PaymentTypeModel>> GetAllAsync()
    {
        return await GetAll()
            .ProjectToType<PaymentTypeModel>()
            .ToListAsync();
    }
}
