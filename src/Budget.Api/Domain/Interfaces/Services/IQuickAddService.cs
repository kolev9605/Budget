using Budget.Api.Domain.Models;

namespace Budget.Api.Domain.Interfaces.Services;

public interface IQuickAddService
{
    Task<List<QuickAddResult>> ParseQuickAddAsync(string inputText, string historyCsv, string categoriesCsv);
}
