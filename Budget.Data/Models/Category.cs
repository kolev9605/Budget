namespace Budget.Data.Models
{
    using Enums;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public TransactionType TransactionType { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        [Required]
        public string RgbColorValue { get; set; }

        public List<UserCategory> UserCategories { get; set; } = new List<UserCategory>();
    }
}