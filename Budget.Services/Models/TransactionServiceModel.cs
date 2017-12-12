namespace Budget.Services.Models
{
    using Budget.Data.Models;
    using System;

    public class TransactionServiceModel
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public User User { get; set; }

        public Category Category { get; set; }

        public string Description { get; set; }
    }
}
