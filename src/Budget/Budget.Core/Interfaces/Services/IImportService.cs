using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IImportService
    {
        Task ImportRecordsAsync(string recordsFileJson, string userId);

        Task<int> ImportWalletRecordsAsync(string walletFileContent, string userId);
    }
}
