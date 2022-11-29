using Budget.Core.Entities;
using Budget.Core.Models.Records;
using System;

namespace Budget.Infrastructure.Factories
{
    public static class RecordFactory
    {
        public static RecordModel ToRecordModel(this Record record)
        {
            if (record == null) return null;

            return new RecordModel
            {
                Id = record.Id,
                Note = record.Note,
                Account = record.Account.ToAccountModel(),
                FromAccount = record.FromAccount.ToAccountModel(),
                RecordType = record.RecordType,
                PaymentType = record.PaymentType.ToPaymentTypeModel(),
                Category = record.Category.ToCategoryModel(),
                DateCreated = record.DateCreated,
                RecordDate = record.RecordDate,
                Amount = record.Amount,
            };
        }

        public static Record ToRecord(this CreateRecordModel createRecordModel, DateTime dateCreated, decimal amount)
        {
            if (createRecordModel == null) return null;
            var record = new Record()
            {
                AccountId = createRecordModel.AccountId,
                Amount = amount,
                Note = createRecordModel.Note,
                DateCreated = dateCreated,
                CategoryId = createRecordModel.CategoryId,
                PaymentTypeId = createRecordModel.PaymentTypeId,
                RecordType = createRecordModel.RecordType,
                RecordDate = createRecordModel.RecordDate,
            };

            return record;
        }
    }
}
