using Budget.Core.Exceptions;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Records;

namespace Budget.Infrastructure.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _recordRepository;

        public RecordService(IRecordRepository recordsRepository)
        {
            _recordRepository = recordsRepository;
        }

        public async Task<IEnumerable<RecordModel>> GetAllAsync()
        {
            var records = await _recordRepository.GetAllWithCurrenciesAsync();

            var recordDtos = records
                .ToList()
                .Select(r => RecordModel.FromRecord(r));

            return recordDtos;
        }

        public async Task<RecordModel> GetByIdAsync(int id)
        {
            var recordEntity = await _recordRepository.GetByIdWithCurrencyAsync(id);

            var recordDto = RecordModel.FromRecord(recordEntity);

            return recordDto;
        }
    }
}
