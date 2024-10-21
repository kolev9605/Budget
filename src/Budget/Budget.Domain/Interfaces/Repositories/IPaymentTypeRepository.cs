using Budget.Domain.Entities;
using Budget.Domain.Models.PaymentTypes;

namespace Budget.Domain.Interfaces.Repositories;

public interface IPaymentTypeRepository : IRepository<PaymentType>
{
    Task<IEnumerable<PaymentTypeModel>> GetAllAsync();
}
