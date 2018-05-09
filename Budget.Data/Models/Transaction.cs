namespace Budget.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Transaction
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
        
        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }
    }
}
