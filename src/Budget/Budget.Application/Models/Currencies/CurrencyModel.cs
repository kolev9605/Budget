using Budget.Domain.Entities;

namespace Budget.Application.Models.Currencies
{
    public class CurrencyModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; }
    }
}
