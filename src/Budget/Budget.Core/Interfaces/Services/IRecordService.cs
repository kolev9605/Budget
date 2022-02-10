using Budget.Core.Entities;
using Budget.Core.Models.Records;

namespace Budget.Core.Interfaces.Services
{
    public interface IRecordService
    {
        Task<IEnumerable<RecordDto>> GetAllAsync();

        Task<RecordDto> GetByIdAsync(int id);
    }
}
