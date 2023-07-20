using Budget.Application.Models.PaymentTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Application.Interfaces.Services
{
    public interface IPaymentTypeService
    {
        Task<IEnumerable<PaymentTypeModel>> GetAllAsync();
    }
}
