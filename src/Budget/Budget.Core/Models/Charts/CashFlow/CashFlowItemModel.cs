using System;

namespace Budget.Core.Models.Charts.CashFlow
{
    public class CashFlowItemModel
    {
        public CashFlowItemModel(decimal sum, DateTime date)
        {
            Sum = sum;
            Date = date;
        }

        public decimal Sum { get; set; }

        public DateTime Date { get; set; }
    }
}
