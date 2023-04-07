using Budget.Application.Models.Currencies;
using Budget.Application.Models.Records;
using Budget.Domain.Entities;
using Mapster;
using System.Collections.Generic;
using System.Linq;

namespace Budget.Application.Models.Accounts
{
    public class AccountModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal InitialBalance { get; set; }

        public decimal Balance => InitialBalance + Records.Select(r => r.Amount).Sum();

        public CurrencyModel Currency { get; set; }

        public IEnumerable<RecordModel> Records { get; set; } = new List<RecordModel>();
    }
}
