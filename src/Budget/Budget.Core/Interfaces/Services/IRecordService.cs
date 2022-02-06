using Budget.Core.Entities;

namespace Budget.Core.Interfaces.Services
{
    public interface IRecordService
    {
        Task<IEnumerable<Record>> GetAll();
    }
}
