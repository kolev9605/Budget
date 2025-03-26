using Budget.Application.Records.Queries;
using Mapster;

namespace Budget.Api.Models.Records;

public record GetRecordByIdForUpdateRequest(Guid Id);

public class GetRecordByIdForUpdateRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetRecordByIdForUpdateRequest GetRecordByIdForUpdateRequest, AuthenticatedUserModel CurrentUser), GetRecordByIdForUpdateQuery>()
            .Map(dest => dest, src => src.GetRecordByIdForUpdateRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
