namespace Budget.Domain.Constants;

public class CacheConstants
{
    public class Currencies
    {
        public const string Key = "currencies";
        public const int ExpirationInSeconds = 60 * 3;
    }

    public class PaymentTypes
    {
        public const string Key = "payment_types";
        public const int ExpirationInSeconds = 60 * 3;
    }

}
