using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Statistics;
using ErrorOr;
using MediatR;

namespace Budget.Application.Statistics.Queries;

public record GetCashFlowStatisticsQuery(
    IEnumerable<Guid> AccountIds,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string UserId) : IRequest<ErrorOr<GetCashFlowStatisticsResult>>;

public class GetCashFlowStatisticsQueryHandler : IRequestHandler<GetCashFlowStatisticsQuery, ErrorOr<GetCashFlowStatisticsResult>>
{
    private readonly IRecordRepository _recordRepository;

    public GetCashFlowStatisticsQueryHandler(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<ErrorOr<GetCashFlowStatisticsResult>> Handle(GetCashFlowStatisticsQuery query, CancellationToken cancellationToken)
    {
        var recordsInRange = await _recordRepository.GetAllInRangeAndAccountsAsync(
            query.UserId,
            query.StartDate,
            query.EndDate, query.AccountIds);

        var income = recordsInRange
            .Where(r => r.Amount > 0)
            .Where(r => r.RecordType == RecordType.Income)
            .Sum(r => r.Amount);

        var expense = recordsInRange
            .Where(r => r.Amount < 0)
            .Where(r => r.RecordType == RecordType.Expense)
            .Sum(r => r.Amount);

        var resultModel = new GetCashFlowStatisticsResult(income, expense);

        return resultModel;
    }
}

