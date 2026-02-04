using Budget.Api.Domain.Entities;
using Budget.Api.Domain.Interfaces.Services;
using Budget.Api.Domain.Models;
using Microsoft.SemanticKernel;

namespace Budget.Api.AI;

public class QuickAddService : IQuickAddService
{
    private readonly Kernel _kernel;
    private readonly KernelFunction _quickAddFunction;
    private readonly ILogger<QuickAddService> _logger;

    public QuickAddService(Kernel kernel, ILogger<QuickAddService> logger)
    {
        var categoryMappingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prompts", "QuickAddPrompt.skprompt.txt");
        var content = File.ReadAllText(categoryMappingFilePath);
        _kernel = kernel;
        _quickAddFunction = _kernel.CreateFunctionFromPrompt(
            content,
            functionName: "QuickAdd"
        );
        _logger = logger;
    }

    public async Task<List<QuickAddResult>> ParseQuickAddAsync(string inputText, string historyCsv, string categoriesCsv)
    {
        var args = new KernelArguments
        {
            ["input"] = inputText,
            ["history"] = historyCsv,
            ["categories"] = categoriesCsv,
            ["today"] = DateTime.UtcNow.ToString("yyyy-MM-dd"),
        };

        var result = await _kernel.InvokeAsync(_quickAddFunction, args);
        var rawLines = result.GetValue<string>()?.Split('\n', StringSplitOptions.RemoveEmptyEntries) ?? [];
        _logger.LogInformation("QuickAdd result: {Result}", string.Join(", ", rawLines));
        var list = new List<QuickAddResult>();
        foreach (var line in rawLines)
        {
            _logger.LogInformation("Parsed line: {Line}", line);
            try
            {
                var parsed = Parse(line);
                _logger.LogInformation("Parsed result: {Parsed}", parsed);
                list.Add(parsed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse line: {Line}", line);
                continue;
            }
        }

        return list;
    }

    private QuickAddResult Parse(string line)
    {
        var parts = line.Trim('"').Split(',');
        return new QuickAddResult
        {
            Amount = decimal.Parse(parts[0].Trim()),
            RecordType = Enum.Parse<RecordType>(parts[1]),
            CategoryName = parts[2].Trim(),
            AccountName = parts[3].Trim(),
            FromAccountName = parts[4].Trim(),
            RecordDate = DateTime.Parse(parts[5].Trim())
        };
    }
}
