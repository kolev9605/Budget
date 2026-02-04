using Budget.Api.Domain.Models.Import.Wallet;

namespace Budget.Api.Domain.Interfaces.Services;

public interface IWalletImportService
{
    IEnumerable<WalletImportModel> ParseWalletCsv(string walletFileContent);
}
