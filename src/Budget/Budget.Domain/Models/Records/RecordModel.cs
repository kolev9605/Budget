using Budget.Domain.Entities;
using Budget.Domain.Models.Accounts;
using Budget.Domain.Models.Categories;
using Budget.Domain.Models.PaymentTypes;
using System;

namespace Budget.Domain.Models.Records;

public class RecordModel
{
    public int Id { get; set; }

    public string Note { get; set; } = null!;

    public AccountModel FromAccount { get; set; } = null!;

    public AccountModel Account { get; set; } = null!;

    public RecordType RecordType { get; set; }

    public PaymentTypeModel PaymentType { get; set; } = null!;

    public CategoryModel Category { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime RecordDate { get; set; }

    public decimal Amount { get; set; }
}
