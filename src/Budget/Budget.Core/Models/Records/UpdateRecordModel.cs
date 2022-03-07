using Budget.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Core.Models.Records
{
    public class UpdateRecordModel
    {
        public int Id { get; set; }

        public string Note { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public int CategoryId { get; set; }

        public int PaymentTypeId { get; set; }

        public RecordType RecordType { get; set; }
    }
}
