namespace Budget.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal? Balance { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public List<UserCategory> UserCategories { get; set; } = new List<UserCategory>();
    }
}
