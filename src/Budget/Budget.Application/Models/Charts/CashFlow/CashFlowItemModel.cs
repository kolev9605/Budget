using System;

namespace Budget.Application.Models.Charts.CashFlow
{
    public class CashFlowItemModel
    {
        public CashFlowItemModel(decimal cashFlow, DateTime date)
        {
            CashFlow = cashFlow;
            Date = date;
        }

        public decimal CashFlow { get; set; }

        public DateTime Date { get; set; }
    }
}
