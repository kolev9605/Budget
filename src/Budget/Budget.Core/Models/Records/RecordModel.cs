using Budget.Core.Entities;
using System;

namespace Budget.Core.Models.Records
{
    public class RecordModel
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public DateTime DateAdded { get; set; }

        public decimal Amount { get; set; }

        public static RecordModel FromRecord(Record record)
        {
            return new RecordModel
            {
                Id = record.Id,
                Amount = record.Amount,
                DateAdded = record.DateAdded,
                Note = record.Note,
            };
        }
    }
}
