using System;

namespace Budget.Domain.Models.Records;

public class RecordsDateRangeModel
{
    public RecordsDateRangeModel(DateTimeOffset minDate, DateTimeOffset maxDate)
    {
        MinDate = minDate;
        MaxDate = maxDate;
    }

    public DateTimeOffset MinDate { get; set; }

    public DateTimeOffset MaxDate { get; set; }
}
