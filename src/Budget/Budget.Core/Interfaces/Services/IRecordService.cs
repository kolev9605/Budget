using Budget.Core.Models.Records;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IRecordService
    {
        Task<RecordModel> GetByIdAsync(int id);

        Task<IEnumerable<RecordModel>> GetAllAsync();

        Task<RecordModel> CreateAsync(CreateRecordModel createRecordModel, string userId);

        Task<RecordModel> UpdateAsync(UpdateRecordModel updateRecordModel, string userId);

        Task<RecordModel> DeleteAsync(int recordId);
    }
}
