namespace Budget.Core.Entities
{
    public class UserCategory
    {
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
