using Mapster;

namespace Budget.Domain.Models.Statistics;

public record GetCashFlowStatisticsResponse(decimal Expense, decimal Income)
{
    public decimal CashFlow => Income - Math.Abs(Expense);
}

public class GetCashFlowStatisticsResponseMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<GetCashFlowStatisticsResult, GetCashFlowStatisticsResponse>().MapToConstructor(true);
    }
}
