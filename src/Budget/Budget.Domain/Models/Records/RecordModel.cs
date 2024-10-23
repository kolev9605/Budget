using Budget.Domain.Entities;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Categories;
using Budget.Domain.Models.PaymentTypes;

namespace Budget.Domain.Models.Records;

public record RecordModel(
    Guid Id,
    string Note,
    // AccountModel FromAccount,
    AccountModel Account,
    RecordType RecordType,
    PaymentTypeModel PaymentType,
    CategoryModel Category,
    DateTimeOffset DateCreated,
    DateTimeOffset RecordDate,
    decimal Amount);
