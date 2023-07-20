namespace Budget.Application.Models.Accounts
{
    public class CreateAccountModel
    {
        public string Name { get; set; }

        public int CurrencyId { get; set; }

        public decimal InitialBalance { get; set; }
    }
}
