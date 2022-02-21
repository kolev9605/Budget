using Budget.Core.Entities;
using Budget.Core.Models.Records;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IRecordService
    {
        Task<IEnumerable<RecordModel>> GetAllAsync();

        Task<RecordModel> GetByIdAsync(int id);
    }
}
