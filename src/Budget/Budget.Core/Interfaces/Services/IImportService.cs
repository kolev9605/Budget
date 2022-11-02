using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IImportService
    {
        Task ImportRecords(string recordsFileJson, string userId);

        Task<string> Parse(string userId);
    }
}
