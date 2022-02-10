using Budget.Core.Entities;

namespace Budget.Core.Interfaces.Repositories
{
    public interface IRecordRepository : IRepository<Record>
    {
        Task<IEnumerable<Record>> GetAllWithCurrenciesAsync();
    }
}
