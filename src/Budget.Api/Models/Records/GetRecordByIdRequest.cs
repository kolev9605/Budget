using Budget.Application.Records.Queries;
using Mapster;

namespace Budget.Api.Models.Records;

public record GetRecordByIdRequest(Guid Id);

public class GetRecordByIdRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetRecordByIdRequest GetRecordByIdRequest, AuthenticatedUserModel CurrentUser), GetRecordByIdQuery>()
            .Map(dest => dest, src => src.GetRecordByIdRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
