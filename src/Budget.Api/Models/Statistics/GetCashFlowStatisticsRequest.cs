using Budget.Application.Statistics.Queries;
using Mapster;

namespace Budget.Api.Models.Statistics;

public record GetCashFlowStatisticsRequest(
    IEnumerable<Guid> AccountIds,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate);

public class GetStatisticsRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetCashFlowStatisticsRequest GetStatisticsRequest, AuthenticatedUserModel CurrentUser), GetCashFlowStatisticsQuery>()
            .Map(dest => dest, src => src.GetStatisticsRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
