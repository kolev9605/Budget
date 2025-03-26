namespace Budget.Domain.Models.Statistics;

public record GetCashFlowStatisticsResult(
    decimal Income,
    decimal Expense);
