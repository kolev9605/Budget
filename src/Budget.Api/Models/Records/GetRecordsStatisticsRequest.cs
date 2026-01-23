using Budget.Application.Records.Queries;
using Mapster;

namespace Budget.Api.Models.Records;

public record GetRecordsStatisticsRequest(
    DateTimeOffset StartDateRange,
    DateTimeOffset EndDateRange);

public class GetStatisticsRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetRecordsStatisticsRequest GetStatisticsRequest, AuthenticatedUserModel CurrentUser), GetRecordsStatisticsQuery>()
            .Map(dest => dest, src => src.GetStatisticsRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
