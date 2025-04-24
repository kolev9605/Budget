namespace Budget.Domain.Models.Statistics;

public record GetCashFlowStatisticsResult(
    decimal TotalBalance,
    decimal Income,
    decimal Expense);
