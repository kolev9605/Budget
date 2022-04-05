using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IImportService
    {
        Task<string> Parse(string userId);
    }
}
