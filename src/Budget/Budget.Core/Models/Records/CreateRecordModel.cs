using Budget.Core.Entities;

namespace Budget.Core.Models.Records
{
    public class CreateRecordModel
    {
        public string Note { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public int CategoryId { get; set; }

        public int PaymentTypeId { get; set; }

        public RecordType RecordType { get; set; }

    }
}
