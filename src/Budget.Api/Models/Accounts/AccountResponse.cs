using Budget.Api.Models.Currencies;

namespace Budget.Api.Models.Accounts;

public record AccountResponse(
    Guid Id,
    string Name,
    decimal InitialBalance,
    decimal Balance,
    CurrencyResponse Currency
);
