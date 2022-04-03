using System;
using System.Collections.Generic;

namespace Budget.Core.Models.Charts.CashFlow
{
    public class CashFlowChartModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal CashFlowForPeriod { get; set; }

        public IEnumerable<CashFlowItemModel> Items { get; set; }
    }
}
