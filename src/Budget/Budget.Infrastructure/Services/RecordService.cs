using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;

namespace Budget.Infrastructure.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRepository<Record> _recordRepository;

        public RecordService(IRepository<Record> recordsRepository)
        {
            _recordRepository = recordsRepository;
        }

        public async Task<IEnumerable<Record>> GetAll()
            => await _recordRepository.AllAsync();
    }
}
