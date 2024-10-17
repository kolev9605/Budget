using Budget.Application.Accounts.Commands.Create;
using Mapster;

namespace Budget.Api.Models.Accounts;

public record CreateAccountRequest(
    string Name,
    Guid CurrencyId,
    decimal InitialBalance
);

public class CreateAccountRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateAccountRequest CreateAccountRequest, AuthenticatedUserModel CurrentUser), CreateAccountCommand>()
            .Map(dest => dest, src => src.CreateAccountRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
