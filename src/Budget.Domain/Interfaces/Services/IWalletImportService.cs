using Budget.Domain.Models.Import.Wallet;

namespace Budget.Domain.Interfaces.Services;

public interface IWalletImportService
{
    IEnumerable<WalletImportModel> ParseWalletCsv(string walletFileContent);
}
