namespace Budget.Application.Models.Authentication
{
    public class RegistrationResultModel
    {
        public RegistrationResultModel(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }
}
