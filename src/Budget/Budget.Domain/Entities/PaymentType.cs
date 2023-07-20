using Budget.Domain.Entities.Base;
using System.Collections.Generic;

namespace Budget.Domain.Entities
{
    public class PaymentType : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Record> Records { get; set; } = new List<Record>();
    }
}
