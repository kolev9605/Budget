namespace Budget.Infrastructure.Authentication;

public class JwtSettings
{
    public const string SectionName = "JWT";

    public string Secret { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;

    public int ExpiryMinutes { get; set; }
}
