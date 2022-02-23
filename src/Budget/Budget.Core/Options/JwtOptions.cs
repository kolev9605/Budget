namespace Budget.Core.Options
{
    public class JwtOptions
    {
        public const string JWT = "JWT";

        public string Secret { get; set; } = string.Empty;
    }
}
