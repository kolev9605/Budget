using Budget.Api.Domain.Exceptions;
using Budget.Api.Domain.Interfaces;
using Budget.Api.Domain.Interfaces.Services;
using Budget.Api.Domain.Models.Import.Wallet;
using Budget.Api.Infrastructure.CsvModels;

namespace Budget.Api.Infrastructure.Services;

public class WalletImportService : IWalletImportService
{
    private ICsvParser _csvParser;

    public WalletImportService(ICsvParser csvParser)
    {
        _csvParser = csvParser;
    }

    public IEnumerable<WalletImportModel> ParseWalletCsv(string walletFileContent)
    {
        var records = _csvParser.ParseCsvString<WalletCsvImportModel>(walletFileContent);
        if (records == null || !records.Any())
        {
            throw new CsvParseException("Invalid CSV format or empty file.");
        }

        return records.Select(r => new WalletImportModel(
            r.Account,
            r.Category,
            r.Currency,
            r.Amount,
            r.RefCurrencyAmount,
            r.Type,
            r.PaymentType,
            r.PaymentTypeLocal,
            r.Note,
            r.Date,
            r.Transfer,
            r.CustomCategory
        ));
    }
}
