using Budget.Core.Entities.Base;
using System;

namespace Budget.Core.Entities
{
    public class Record : IBaseEntity
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime RecordDate { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }

        public int PaymentTypeId { get; set; }

        public PaymentType PaymentType { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public RecordType RecordType { get; set; }
    }
}
