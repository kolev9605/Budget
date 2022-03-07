using Budget.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Repositories
{
    public interface IRecordRepository : IRepository<Record>
    {
        Task<IEnumerable<Record>> GetAllAsync();

        Task<Record> GetRecordByIdAsync(int id);
    }
}
