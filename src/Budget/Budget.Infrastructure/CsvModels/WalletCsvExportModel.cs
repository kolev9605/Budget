using CsvHelper.Configuration.Attributes;
using System;

namespace Budget.Infrastructure.CsvModels;

public class WalletCsvExportModel
{
    [Name("account")]
    public string Account { get; set; }

    [Name("category")]
    public string Category { get; set; }

    [Name("currency")]
    public string Currency { get; set; }

    [Name("amount")]
    public decimal Amount { get; set; }

    [Name("ref_currency_amount")]
    public decimal RefCurrencyAmount { get; set; }

    [Name("type")]
    public string Type { get; set; }

    [Name("payment_type")]
    public string PaymentType { get; set; }

    [Name("payment_type_local")]
    public string PaymentTypeLocal { get; set; }

    [Name("note")]
    public string Note { get; set; }

    [Name("date")]
    public DateTime Date { get; set; }

    [Name("transfer")]
    public bool Transfer { get; set; }

    [Name("custom_category")]
    public bool CustomCategory { get; set; }
}
