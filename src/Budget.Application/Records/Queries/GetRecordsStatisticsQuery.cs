using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Records.Statistics;
using ErrorOr;
using MediatR;

namespace Budget.Application.Records.Queries;

public record GetRecordsStatisticsQuery(
    DateTimeOffset StartDateRange,
    DateTimeOffset EndDateRange,
    string UserId) : IRequest<ErrorOr<IEnumerable<GetRecordsStatisticsResult>>>;

public class GetRecordsStatisticsQueryHandler : IRequestHandler<GetRecordsStatisticsQuery, ErrorOr<IEnumerable<GetRecordsStatisticsResult>>>
{
    private readonly IRecordRepository _recordRepository;
    private readonly IAccountRepository _accountRepository;

    public GetRecordsStatisticsQueryHandler(IRecordRepository recordRepository, IAccountRepository accountRepository)
    {
        _recordRepository = recordRepository;
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<IEnumerable<GetRecordsStatisticsResult>>> Handle(GetRecordsStatisticsQuery query, CancellationToken cancellationToken)
    {
        var recordsInRange = await _recordRepository.GetCashFlowStatisticsAsync(
            query.UserId,
            query.StartDateRange.UtcDateTime,
            query.EndDateRange.UtcDateTime);

        return recordsInRange.ToErrorOr();
    }
}

