using Budget.Application.Accounts.Queries.GetAll;
using Mapster;

namespace Budget.Api.Models;

public record AuthenticatedUserModel(
    string Id,
    string Email);

public class AuthenticatedUserModelMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticatedUserModel, GetAllAccountsQuery>()
            .Map(dest => dest.UserId, src => src.Id);
    }
}
