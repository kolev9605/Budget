using Budget.Core.Models.Records;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IRecordService
    {
        Task<RecordModel> GetByIdAsync(int id, string userId);

        Task<IEnumerable<RecordsGroupModel>> GetAllAsync(string userId);

        Task<int> CreateAsync(CreateRecordModel createRecordModel, string userId);

        Task<int> UpdateAsync(UpdateRecordModel updateRecordModel, string userId);

        Task<int> DeleteAsync(int recordId, string userId);
    }
}
