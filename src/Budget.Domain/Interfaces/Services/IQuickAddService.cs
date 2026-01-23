using Budget.Domain.Models;

namespace Budget.Domain.Interfaces.Services;

public interface IQuickAddService
{
    Task<List<QuickAddResult>> ParseQuickAddAsync(string inputText, string historyCsv, string categoriesCsv);
}
