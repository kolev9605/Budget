namespace Budget.Api.Models.Currencies;

public record CurrencyResponse(
    int Id,
    string Name,
    string Abbreviation,
    // What is this?
    string SpecialName
);
