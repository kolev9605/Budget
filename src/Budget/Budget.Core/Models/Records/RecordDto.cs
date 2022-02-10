using Budget.Core.Entities;
using Budget.Core.Models.Currencies;

namespace Budget.Core.Models.Records
{
    public class RecordDto
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public DateTime DateAdded { get; set; }

        public decimal Amount { get; set; }

        public CurrencyDto Currency { get; set; }

        public static RecordDto FromRecord(Record record)
        {
            return new RecordDto
            {
                Id = record.Id,
                Amount = record.Amount,
                Currency = new CurrencyDto
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
