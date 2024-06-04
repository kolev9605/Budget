using Budget.Domain.Entities;
using Budget.Domain.Models.Currencies;
using Mapster;

namespace Budget.Domain.Models.Accounts;

public record AccountModel(
    int Id,
    string Name,
    decimal InitialBalance,
    decimal Balance,
    CurrencyModel Currency);

public class AccountMappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Account, AccountModel>()
            .MaxDepth(2)
            .Map(dest => dest.Balance, src => src.InitialBalance + src.Records.Select(r => r.Amount).Sum());
    }
}
