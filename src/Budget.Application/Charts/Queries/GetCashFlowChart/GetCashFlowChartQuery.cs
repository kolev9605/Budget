using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Models.Charts.CashFlow;
using ErrorOr;
using MediatR;

namespace Budget.Application.Charts.Queries.GetCashFlowChart;

public record GetCashFlowChartQuery(
    List<Guid> AccountIds,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string UserId) : IRequest<ErrorOr<CashFlowChartModel?>>;

public class GetCashFlowChartQueryHandler : IRequestHandler<GetCashFlowChartQuery, ErrorOr<CashFlowChartModel?>>
{
    private readonly IRecordRepository _recordRepository;

    public GetCashFlowChartQueryHandler(
        IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    public async Task<ErrorOr<CashFlowChartModel?>> Handle(GetCashFlowChartQuery query, CancellationToken cancellationToken)
    {
        var records = await _recordRepository.GetAllInRangeAndAccountsAsync(query.UserId, query.StartDate, query.EndDate, query.AccountIds);

        if (!records.Any())
        {
            // TODO: See if we can return something other than null
            return ErrorOrFactory.From((CashFlowChartModel?)null);
        }

        var cashFlowItems = records
            .GroupBy(r => r.RecordDate.Date)
            .ToDictionary(r => r.Key, r => r.Sum(v => v.Amount))
            .Select(r => new CashFlowItemModel(GetCashFlow(records, r.Key), r.Key))
            .ToList();

        var chartData = new CashFlowChartModel(cashFlowItems.Min(r => r.Date), cashFlowItems.Max(r => r.Date), records.Sum(r => r.Amount), cashFlowItems);

        return chartData;
    }

    private decimal GetCashFlow(IEnumerable<Record> records, DateTime date)
        => records
            .Where(r => r.RecordDate.Date <= date.Date)
            .Sum(r => r.Amount);
}
