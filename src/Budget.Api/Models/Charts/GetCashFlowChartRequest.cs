using Budget.Application.Charts.Queries.GetCashFlowChart;
using Mapster;

namespace Budget.Api.Models.Charts;

public record GetCashFlowChartRequest(
    List<Guid> AccountIds,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);

public class CashFlowChartRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetCashFlowChartRequest CashFlowChartRequest, AuthenticatedUserModel CurrentUser), GetCashFlowChartQuery>()
            .Map(dest => dest, src => src.CashFlowChartRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
