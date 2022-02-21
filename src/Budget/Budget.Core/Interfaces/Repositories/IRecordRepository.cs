using Budget.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Repositories
{
    public interface IRecordRepository : IRepository<Record>
    {
        Task<IEnumerable<Record>> GetAllWithCurrenciesAsync();

        Task<Record> GetByIdWithCurrencyAsync(int id);
    }
}
