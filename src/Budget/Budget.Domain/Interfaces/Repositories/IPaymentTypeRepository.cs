using Budget.Domain.Entities;
using Budget.Domain.Models.PaymentTypes;
using Budget.Domain.Models.Records.Create;

namespace Budget.Domain.Interfaces.Repositories;

public interface IPaymentTypeRepository : IRepository<PaymentType>
{
    Task<IEnumerable<PaymentTypeModel>> GetAllAsync();

    Task<PaymentTypeForRecordCreationModel?> GetForRecordCreationAsync(Guid id);
}
