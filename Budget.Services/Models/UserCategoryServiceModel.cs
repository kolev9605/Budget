namespace Budget.Services.Models
{
    using Budget.Data.Models;
    using Budget.Data.Models.Enums;
    using System.Collections.Generic;

    public class UserCategoryServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TransactionType TransactionType { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public List<UserCategory> UserCategories { get; set; } = new List<UserCategory>();

        public bool IsPrimary { get; set; }

        public string UserId { get; set; }
    }
}
