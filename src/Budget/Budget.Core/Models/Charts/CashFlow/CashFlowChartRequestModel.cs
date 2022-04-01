using System.Collections.Generic;

namespace Budget.Core.Models.Charts.CashFlow
{
    public class CashFlowChartRequestModel
    {
        public IEnumerable<int> AccountIds { get; set; } = new List<int>();

        public int Month { get; set; }
    }
}
