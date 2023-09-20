using System;
using System.Collections.Generic;

namespace Budget.Domain.Models.Records;

public class RecordsGroupModel
{
    public DateTime Date { get; set; }

    public decimal Sum { get; set; }

    public IEnumerable<RecordModel> Records { get; set; } = new List<RecordModel>();
}
