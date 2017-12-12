namespace Budget.Web.Areas.User.ViewModels
{
    using Budget.Data.Models.Enums;
    using System;

    public class TransactionDataViewModel
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public string CategoryName { get; set; }

        public TransactionType TransactionType { get; set; }

        public string CategoryDescription { get; set; }
    }
}
