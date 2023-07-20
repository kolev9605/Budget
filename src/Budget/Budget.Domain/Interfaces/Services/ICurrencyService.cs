using Budget.Domain.Models.Currencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyModel>> GetAllAsync();
    }
}
