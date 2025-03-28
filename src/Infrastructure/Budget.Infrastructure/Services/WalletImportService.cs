using Budget.Domain.Exceptions;
using Budget.Domain.Interfaces;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Import.Wallet;
using Budget.Infrastructure.CsvModels;
using Mapster;

namespace Budget.Infrastructure.Services;

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

        return records.Adapt<IEnumerable<WalletImportModel>>();
    }
}
