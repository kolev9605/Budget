using System;
using System.Collections.Generic;

namespace Budget.Domain.Models.Charts.CashFlow
{
    public class CashFlowChartRequestModel
    {
        public IEnumerable<int> AccountIds { get; set; } = new List<int>();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
