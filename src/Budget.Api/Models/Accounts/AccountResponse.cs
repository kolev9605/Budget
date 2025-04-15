using Budget.Api.Models.Currencies;
using Budget.Api.Models.PaymentTypes;

namespace Budget.Api.Models.Accounts;

public record AccountResponse(
    Guid Id,
    string Name,
    decimal InitialBalance,
    decimal Balance,
    CurrencyResponse Currency,
    PaymentTypeResponse PaymentType
);
