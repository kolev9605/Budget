using Budget.Core.Entities;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Currencies;
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

        public async Task<IEnumerable<RecordDto>> GetAllAsync()
        {
            var records = await _recordRepository.GetAllWithCurrenciesAsync();

            var recordDtos = records
                .ToList()
                .Select(r => RecordDto.FromRecord(r));

            return recordDtos;
        }

        public async Task<RecordDto> GetByIdAsync(int id)
        {
            var recordEntity = await _recordRepository.GetByIdWithCurrencyAsync(id);

            var recordDto = RecordDto.FromRecord(recordEntity);

            return recordDto;
        }
    }
}
