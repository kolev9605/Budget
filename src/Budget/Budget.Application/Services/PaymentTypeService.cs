using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.PaymentTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly IRepository<PaymentType> _paymentTypeRepository;

        public PaymentTypeService(IRepository<PaymentType> paymentTypeRepository)
        {
            _paymentTypeRepository = paymentTypeRepository;
        }

        public async Task<IEnumerable<PaymentTypeModel>> GetAllAsync()
        {
            var paymentTypes = await _paymentTypeRepository.BaseGetAllAsync<PaymentTypeModel>();

            return paymentTypes;
        }
    }
}
