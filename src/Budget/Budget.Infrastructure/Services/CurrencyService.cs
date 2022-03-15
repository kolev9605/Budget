using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Currencies;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budget.Infrastructure.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository<Currency> _repository;

        public CurrencyService(IRepository<Currency> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CurrencyModel>> GetAllAsync()
        {
            var currencies = await _repository.BaseAllAsync();

            var currencyModels = currencies
                .Select(c => new CurrencyModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Abbreviation = c.Abbreviation,
                });

            return currencyModels;
        }
    }
}
