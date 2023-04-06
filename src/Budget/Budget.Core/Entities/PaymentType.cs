using Budget.Core.Entities.Base;
using System.Collections.Generic;

namespace Budget.Core.Entities
{
    public class PaymentType : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Record> Records { get; set; } = new List<Record>();
    }
}
