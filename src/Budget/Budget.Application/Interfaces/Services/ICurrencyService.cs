using Budget.Application.Models.Currencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Application.Interfaces.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyModel>> GetAllAsync();
    }
}
