using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Currencies;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository<Currency> _currencyRepository;

        public CurrencyService(IRepository<Currency> currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<IEnumerable<CurrencyModel>> GetAllAsync()
        {
            var currencies = await _currencyRepository.BaseGetAllAsync();

            return currencies.Adapt<IEnumerable<CurrencyModel>>();

        }
    }
}
