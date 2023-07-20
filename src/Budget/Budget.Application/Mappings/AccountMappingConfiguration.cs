using Budget.Domain.Entities;
using Budget.Domain.Models.Accounts;
using Mapster;
using System.Linq;

namespace Budget.Application.Mappings;

public class AccountMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Account, AccountModel>()
            .MaxDepth(2)
            .Map(dest => dest.Balance, src => src.InitialBalance + src.Records.Select(r => r.Amount).Sum());

        config.NewConfig<(CreateAccountModel Account, string UserId), Account>()
          .Map(dest => dest.UserId, src => src.UserId)
          .Map(dest => dest, src => src.Account);
    }
}
