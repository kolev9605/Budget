using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;

namespace Budget.Infrastructure.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _recordRepository;

        public RecordService(IRecordRepository recordsRepository)
        {
            _recordRepository = recordsRepository;
        }

        public async Task<IEnumerable<Record>> GetAllAsync()
            => await _recordRepository.GetAllWithCurrenciesAsync();
    }
}
