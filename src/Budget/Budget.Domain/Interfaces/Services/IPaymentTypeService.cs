using Budget.Domain.Models.PaymentTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Services
{
    public interface IPaymentTypeService
    {
        Task<IEnumerable<PaymentTypeModel>> GetAllAsync();
    }
}
