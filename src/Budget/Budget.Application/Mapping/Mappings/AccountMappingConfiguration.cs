using Budget.Application.Models.Accounts;
using Budget.Domain.Entities;
using Mapster;

namespace Budget.Application.Mapping;

public class AccountMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Account, AccountModel>().MaxDepth(2);

        config.NewConfig<(CreateAccountModel Account, string UserId), Account>()
          .Map(dest => dest.UserId, src => src.UserId)
          .Map(dest => dest, src => src.Account);
    }
}