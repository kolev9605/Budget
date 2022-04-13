using System;

namespace Budget.Core.Models.Statistics
{
    public class StatisticsResultModel
    {
        public StatisticsResultModel(decimal income, decimal expense)
        {
            Income = income;
            Expense = expense;
        }

        public decimal CashFlow => Income - Math.Abs(Expense);

        public decimal Expense { get; set; }

        public decimal Income { get; set; }
    }
}
