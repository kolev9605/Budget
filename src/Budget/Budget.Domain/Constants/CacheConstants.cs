namespace Budget.Domain.Constants;

public class CacheConstants
{
    public class Keys
    {
        public const string Currencies = "Currencies";
    }

    public class Expirations
    {
        public const int CurrenciesExpirationInSeconds = 60 * 3;
    }

}
