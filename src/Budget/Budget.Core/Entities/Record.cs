using Budget.Core.Entities.Base;

namespace Budget.Core.Entities
{
    public class Record : BaseEntity
    {
        public string Note { get; set; }

        public DateTime DateAdded { get; set; }

        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public int CurrencyId { get; set; }
    }
}
