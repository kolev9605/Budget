using System;
using System.Collections.Generic;

namespace Budget.Core.Models.Records
{
    public class RecordListModel
    {
        public DateTime Date { get; set; }

        public IEnumerable<RecordModel> Records { get; set; } = new List<RecordModel>();
    }
}
