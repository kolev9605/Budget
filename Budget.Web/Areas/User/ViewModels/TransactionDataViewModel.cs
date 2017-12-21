namespace Budget.Web.Areas.User.ViewModels
{
    using System;

    public class TransactionDataViewModel
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }
    }
}
