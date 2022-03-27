using Budget.Core.Entities.Base;
using System.Collections.Generic;

namespace Budget.Core.Entities
{
    public class Account : IBaseEntity
    {
        public int Id { get; set; }

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
