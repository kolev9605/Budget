using Budget.Domain.Common.Errors;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Records;
using ErrorOr;
using MediatR;

namespace Budget.Application.Records.Queries;

public record GetRecordsDateRangeQuery(string UserId) : IRequest<ErrorOr<RecordsDateRangeResult>>;

public class GetRecordsDateRangeQueryHandler : IRequestHandler<GetRecordsDateRangeQuery, ErrorOr<RecordsDateRangeResult>>
{
    private readonly IRecordRepository _recordRepository;

    public GetRecordsDateRangeQueryHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<ErrorOr<RecordsDateRangeResult>> Handle(GetRecordsDateRangeQuery query, CancellationToken cancellationToken)
    {
        var result = _recordRepository.GetDateRangeByUser(query.UserId);
        if (result is null)
        {
            return Errors.Record.NoRecords;
        }

        return result;
    }
}
