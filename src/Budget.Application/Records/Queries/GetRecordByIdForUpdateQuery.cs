using Budget.Domain.Common.Errors;
using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Records;
using ErrorOr;
using MediatR;

namespace Budget.Application.Records.Queries;

public record GetRecordByIdForUpdateQuery(
    Guid Id,
    string UserId) : IRequest<ErrorOr<RecordModel>>;

public class GetRecordByIdForUpdateQueryHandler : IRequestHandler<GetRecordByIdForUpdateQuery, ErrorOr<RecordModel>>
{
    private readonly IRecordRepository _recordRepository;

    public GetRecordByIdForUpdateQueryHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<ErrorOr<RecordModel>> Handle(GetRecordByIdForUpdateQuery query, CancellationToken cancellationToken)
    {
        var record = await _recordRepository.GetRecordByIdMappedAsync(query.Id, query.UserId);
        if (record is null)
        {
            return Errors.Record.NotFound;
        }

        // Only the positive transfer record should be edited to simplify the update process
        if (record.RecordType == RecordType.Transfer)
        {
            var positiveTransferRecord = await _recordRepository.GetPositiveTransferRecordMappedAsync(record.RecordDate, record.Category.Id, record.Amount);
            if (positiveTransferRecord is null)
            {
                return Errors.Record.NotFound;
            }

            return positiveTransferRecord;
        }

        return record;
    }
}
