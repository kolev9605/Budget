using Budget.Domain.Entities.Base;
using System.Collections.Generic;

namespace Budget.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Record> Records { get; set; } = new List<Record>();

        public ICollection<Record> TransferRecords { get; set; } = new List<Record>();

        public int CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public decimal InitialBalance { get; set; }
    }
}
