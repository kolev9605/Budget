namespace Budget.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public const string SectionName = "JWT";

        public string Secret { get; set; } = string.Empty;
    }
}
