using Budget.Application.Accounts.Commands.Create;
using Mapster;

namespace Budget.Api.Models.Accounts;

public record UpdateAccountRequest(
    Guid Id,
    string Name,
    Guid CurrencyId,
    decimal InitialBalance
);

public class UpdateAccountRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(UpdateAccountRequest UpdateAccountRequest, AuthenticatedUserModel CurrentUser), UpdateAccountCommand>()
            .Map(dest => dest, src => src.UpdateAccountRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}

