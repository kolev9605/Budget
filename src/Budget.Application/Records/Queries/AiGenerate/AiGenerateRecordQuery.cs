using Budget.Domain.Entities;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models;
using Budget.Domain.Models.Records;
using ErrorOr;
using MediatR;

namespace Budget.Application.Records.Queries.AiGenerate;

public record AiGenerateRecordQuery(string Prompt, string UserId) : IRequest<ErrorOr<AiGenerateRecordQueryResult?>>;

public record AiGenerateRecordQueryResult(
    string CategoryName,
    Guid CategoryId,
    string AccountName,
    Guid AccountId,
    string? FromAccountName,
    Guid? FromAccountId,
    decimal Amount,
    DateTime RecordDate,
    string? Note,
    RecordType RecordType)
{
    public static AiGenerateRecordQueryResult FromQuickAddResult(
        QuickAddResult result,
        Guid CategoryId,
        Guid AccountId,
        Guid? FromAccountId = null)
    {
        return new AiGenerateRecordQueryResult(
            result.CategoryName ?? string.Empty,
            CategoryId,
            result.AccountName ?? string.Empty,
            AccountId,
            result.FromAccountName,
            FromAccountId,
            result.Amount,
            result.RecordDate,
            result.Note,
            result.RecordType);
    }
};

public class AiGenerateRecordQueryHandler : IRequestHandler<AiGenerateRecordQuery, ErrorOr<AiGenerateRecordQueryResult?>>
{
    private readonly IQuickAddService _quickAddService;
    private readonly IRecordRepository _recordRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;

    public AiGenerateRecordQueryHandler(
        IQuickAddService quickAddService,
        IRecordRepository recordRepository,
        ICategoryRepository categoryRepository,
        IAccountRepository accountRepository)
    {
        _quickAddService = quickAddService;
        _recordRepository = recordRepository;
        _categoryRepository = categoryRepository;
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<AiGenerateRecordQueryResult?>> Handle(AiGenerateRecordQuery query, CancellationToken cancellationToken)
    {
        var last100records = await _recordRepository.GetLastRecordsAsync(query.UserId, 100);
        var historyCsv = string.Join(Environment.NewLine, last100records.Select(r => r.ToCsv()));

        var categories = await _categoryRepository.GetAllAsync(query.UserId);
        var categoriesCsv = string.Join(", ", categories.Select(c => c.Name));

        var response = await _quickAddService.ParseQuickAddAsync(query.Prompt, historyCsv, categoriesCsv);
        if (response.Count == 0)
        {
            return (AiGenerateRecordQueryResult?)null;
        }

        var accounts = await _accountRepository.GetAllAccountModelsByUserIdAsync(query.UserId);

        var firstResult = response[0];
        var category = categories.FirstOrDefault(c => c.Name.Equals(firstResult.CategoryName, StringComparison.OrdinalIgnoreCase));
        var account = accounts.FirstOrDefault(a => a.Name.Equals(firstResult.AccountName, StringComparison.OrdinalIgnoreCase));
        var fromAccount = accounts.FirstOrDefault(a => a.Name.Equals(firstResult.FromAccountName, StringComparison.OrdinalIgnoreCase));

        // TODO: Return error
        if (category == null || account == null)
        {
            return (AiGenerateRecordQueryResult?)null;
        }

        var result = AiGenerateRecordQueryResult.FromQuickAddResult(
            firstResult,
            category.Id,
            account.Id,
            fromAccount?.Id);

        return result;
    }
}
