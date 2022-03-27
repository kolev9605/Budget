using Budget.Core.Entities;
using Budget.Core.Models.Accounts;
using Budget.Core.Models.Categories;
using Budget.Core.Models.PaymentTypes;
using System;

namespace Budget.Core.Models.Records
{
    public class RecordModel
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public AccountModel FromAccount { get; set; }

        public AccountModel Account { get; set; }

        public RecordType RecordType { get; set; }

        public PaymentTypeModel PaymentType { get; set; }

        public CategoryModel Category { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime RecordDate { get; set; }

        public decimal Amount { get; set; }

        public static RecordModel FromRecord(Record record)
        {
            return new RecordModel
            {
                Id = record.Id,
                Note = record.Note,
                Account = AccountModel.FromAccount(record.Account),
                FromAccount = record.FromAccountId.HasValue ? AccountModel.FromAccount(record.FromAccount) : null,
                RecordType = record.RecordType,
                PaymentType = PaymentTypeModel.FromPaymentType(record.PaymentType),
                Category = CategoryModel.FromCategory(record.Category),
                DateCreated = record.DateCreated,
                RecordDate = record.RecordDate,
                Amount = record.Amount,
            };
        }
    }
}
