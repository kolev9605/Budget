namespace Budget.Api.Models.Currencies;

public record CurrencyResponse(
    Guid Id,
    string Name,
    string Abbreviation
);
