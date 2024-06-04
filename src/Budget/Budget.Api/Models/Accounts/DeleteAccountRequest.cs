using Budget.Application.Accounts.Commands.Create;
using Mapster;

namespace Budget.Api.Models.Accounts;

public record DeleteAccountRequest(int AccountId);

public class DeleteAccountRequestMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(DeleteAccountRequest DeleteAccountRequest, AuthenticatedUserModel CurrentUser), DeleteAccountCommand>()
            .Map(dest => dest, src => src.DeleteAccountRequest)
            .Map(dest => dest.UserId, src => src.CurrentUser.Id);
    }
}
