using Budget.Application.Records.Queries;
using Mapster;

namespace Budget.Api.Models.Records;

public record GetAllRecordsRequest(
    int PageNumber,
    int? PageSize);

public class GetAllRecordsRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetAllRecordsRequest GetAllRecordsRequest, AuthenticatedUserModel CurrentUser), GetAllRecordsQuery>()
            .Map(dest => dest, src => src.GetAllRecordsRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
