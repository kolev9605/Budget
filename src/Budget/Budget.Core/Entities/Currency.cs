using Budget.Core.Entities.Base;

namespace Budget.Core.Entities
{
    public class Currency : BaseEntity
    {
        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public ICollection<Record> Records { get; set; } = new List<Record>();
    }
}
