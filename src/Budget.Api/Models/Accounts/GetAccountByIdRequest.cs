using Budget.Application.Accounts.Queries.GetById;
using Mapster;

namespace Budget.Api.Models.Accounts;

public record GetAccountByIdRequest(Guid AccountId);

public class GetAccountByIdRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(GetAccountByIdRequest GetAccountByIdRequest, AuthenticatedUserModel CurrentUser), GetAccountByIdQuery>()
            .Map(dest => dest, src => src.GetAccountByIdRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
