using Budget.Domain.Entities;
using Budget.Domain.Models.Currencies;

namespace Budget.Domain.Interfaces.Repositories;

public interface ICurrencyRepository : IRepository<Currency>
{
    Task<IEnumerable<CurrencyModel>> GetAllItems();
}
