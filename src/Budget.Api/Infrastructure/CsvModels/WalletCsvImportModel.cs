using CsvHelper.Configuration.Attributes;

namespace Budget.Api.Infrastructure.CsvModels;

public class WalletCsvImportModel
{
    [Name("account")]
    public string Account { get; set; } = null!;

    [Name("category")]
    public string Category { get; set; } = null!;

    [Name("currency")]
    public string Currency { get; set; } = null!;

    [Name("amount")]
    public decimal Amount { get; set; }

    [Name("ref_currency_amount")]
    public decimal RefCurrencyAmount { get; set; }

    [Name("type")]
    public string Type { get; set; } = null!;

    [Name("payment_type")]
    public string PaymentType { get; set; } = null!;

    [Name("payment_type_local")]
    public string PaymentTypeLocal { get; set; } = null!;

    [Name("note")]
    public string Note { get; set; } = null!;

    [Name("date")]
    public DateTime Date { get; set; }

    [Name("transfer")]
    public bool Transfer { get; set; }

    [Name("custom_category")]
    public bool CustomCategory { get; set; }
}
