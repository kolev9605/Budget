using Budget.Core.Entities;
using Budget.Core.Models.Currencies;
using System;

namespace Budget.Core.Models.Records
{
    public class RecordModel
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public DateTime DateAdded { get; set; }

        public decimal Amount { get; set; }

        public CurrencyModel Currency { get; set; }

        public static RecordModel FromRecord(Record record)
        {
            return new RecordModel
            {
                Id = record.Id,
                Amount = record.Amount,
                Currency = new CurrencyModel
                {
                    Id = record.Currency.Id,
                    Name = record.Currency.Name,
                    Abbreviation = record.Currency.Abbreviation,
                },
                DateAdded = record.DateAdded,
                Note = record.Note,
            };
        }
    }
}
