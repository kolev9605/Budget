using Budget.Core.Entities.Base;
using System.Collections.Generic;

namespace Budget.Core.Entities
{
    public class Currency : IBaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
