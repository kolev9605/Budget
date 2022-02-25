using Budget.Core.Models.Currencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyModel>> GetAllAsync();
    }
}
