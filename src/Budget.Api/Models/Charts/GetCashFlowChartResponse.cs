namespace Budget.Api.Models.Charts;

public record CashFlowChartResponse(
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    decimal CashFlowForPeriod,
    List<CashFlowItemResponse> Items);

public record CashFlowItemResponse(
    decimal CashFlow,
    DateTimeOffset Date);
