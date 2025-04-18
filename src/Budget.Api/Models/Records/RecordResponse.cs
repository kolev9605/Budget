using Budget.Api.Models.Accounts;
using Budget.Api.Models.Categories;
using Budget.Domain.Entities;

namespace Budget.Api.Models.Records;

public record RecordResponse(
    Guid Id,
    string Note,
    AccountResponse FromAccount,
    AccountResponse Account,
    RecordType RecordType,
    CategoryResponse Category,
    DateTimeOffset DateCreated,
    DateTimeOffset RecordDate,
    decimal Amount);
