namespace Budget.Infrastructure.Authentication
{
    public class JwtOptions
    {
        public const string JWT = "JWT";

        public string Secret { get; set; } = string.Empty;
    }
}
