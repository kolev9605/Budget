using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.PaymentTypes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly IRepository<PaymentType> _paymentTypesRepository;

        public PaymentTypeService(IRepository<PaymentType> paymentTypesRepository)
        {
            _paymentTypesRepository = paymentTypesRepository;
        }

        public async Task<IEnumerable<PaymentTypeModel>> GetAllAsync()
        {
            var paymentTypes = await _paymentTypesRepository.AllAsync();

            var paymentTypeModels = paymentTypes.Select(pt => new PaymentTypeModel()
            {
                Id = pt.Id,
                Name = pt.Name,
            });

            return paymentTypeModels;
        }
    }
}
