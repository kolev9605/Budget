using Budget.Domain.Entities.Base;
using System;

namespace Budget.Domain.Entities
{
    public class Record : BaseEntity
    {
        public string Note { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime RecordDate { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }

        public int? FromAccountId { get; set; }

        public Account FromAccount { get; set; }

        public int PaymentTypeId { get; set; }

        public PaymentType PaymentType { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public RecordType RecordType { get; set; }
    }
}
