using Budget.Api.Models.Currencies;

namespace Budget.Api.Models.Accounts;

public record AccountResponse(
    int Id,
    string Name,
    decimal InitialBalance,
    decimal Balance,
    CurrencyResponse Currency
);
