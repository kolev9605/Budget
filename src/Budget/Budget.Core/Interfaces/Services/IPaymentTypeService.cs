using Budget.Core.Models.PaymentTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IPaymentTypeService
    {
        Task<IEnumerable<PaymentTypeModel>> GetAllAsync();
    }
}
