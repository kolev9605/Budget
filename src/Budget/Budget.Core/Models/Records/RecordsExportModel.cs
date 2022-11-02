using Budget.Core.Entities;
using System;
using Budget.Common;

namespace Budget.Core.Models.Records
{
    public class RecordsExportModel
    {
        public string Note { get; set; }

        public string FromAccount { get; set; }

        public string Account { get; set; }

        public RecordType RecordType { get; set; }

        public string PaymentType { get; set; }

        public string Category { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime RecordDate { get; set; }

        public decimal Amount { get; set; }

        public static RecordsExportModel FromRecord(Record record)
        {
            return new RecordsExportModel
            {
                Note = record.Note,
                Account = record.Account.Name,
                FromAccount = record.FromAccountId.HasValue ? record.FromAccount.Name : null,
                RecordType = record.RecordType,
                PaymentType = record.PaymentType.Name,
                Category = record.Category.Name,
                DateCreated = record.DateCreated,
                RecordDate = record.RecordDate,
                Amount = record.Amount,
            };
        }

        public static Record ToRecord(RecordsExportModel record)
        {
            return new Record
            {
                Note = record.Note,
                RecordType = record.RecordType,
                DateCreated = record.DateCreated.SetKindUtc(),
                RecordDate = record.RecordDate.SetKindUtc(),
                Amount = record.Amount,
            };
        }
    }
}
