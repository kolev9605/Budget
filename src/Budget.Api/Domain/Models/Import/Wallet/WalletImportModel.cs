namespace Budget.Api.Domain.Models.Import.Wallet;

public record WalletImportModel(
    string Account,
    string Category,
    string Currency,
    decimal Amount,
    decimal RefCurrencyAmount,
    string Type,
    string PaymentType,
    string PaymentTypeLocal,
    string Note,
    DateTime Date,
    bool Transfer,
    bool CustomCategory
);
