using Budget.Api.Models.Accounts;
using Budget.Api.Models.Categories;
using Budget.Api.Models.PaymentTypes;
using Budget.Domain.Entities;

namespace Budget.Api.Models.Records;

public record RecordResponse(
    Guid Id,
    string Note,
    AccountResponse FromAccount,
    AccountResponse Account,
    RecordType RecordType,
    PaymentTypeResponse PaymentType,
    CategoryResponse Category,
    DateTimeOffset DateCreated,
    DateTimeOffset RecordDate,
    decimal Amount);
