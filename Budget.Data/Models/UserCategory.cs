namespace Budget.Data.Models
{
    public class UserCategory
    {
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
