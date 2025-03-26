namespace Budget.Domain.Models.Charts.CashFlow;

public record CashFlowChartModel(
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    decimal CashFlowForPeriod,
    List<CashFlowItemModel> Items);

public record CashFlowItemModel(
    decimal CashFlow,
    DateTimeOffset Date);
