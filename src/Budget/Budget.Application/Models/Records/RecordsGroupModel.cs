using Budget.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Budget.Application.Models.Records
{
    public class RecordsGroupModel
    {
        public DateTime Date { get; set; }

        public decimal Sum { get; set; }

        public IEnumerable<RecordModel> Records { get; set; } = new List<RecordModel>();
    }
}
