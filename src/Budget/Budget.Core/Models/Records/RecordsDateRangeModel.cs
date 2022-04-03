using System;

namespace Budget.Core.Models.Records
{
    public class RecordsDateRangeModel
    {
        public RecordsDateRangeModel(DateTime minDate, DateTime maxDate)
        {
            MinDate = minDate;
            MaxDate = maxDate;
        }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }
    }
}
