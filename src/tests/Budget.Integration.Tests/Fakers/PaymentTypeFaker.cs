using Bogus;
using Budget.Api.Domain.Entities;

namespace Budget.Integration.Tests.Fakers;

public class PaymentTypeFaker : Faker<PaymentType>
{
    public PaymentTypeFaker()
    {
        RuleFor(p => p.Id, f => Guid.NewGuid());
        RuleFor(p => p.Name, f => f.PickRandom(new[]
        {
            "Cash",
            "Credit Card",
            "Debit Card",
            "Bank Transfer",
            "PayPal",
            "Venmo",
            "Apple Pay",
            "Google Pay",
            "Check",
            "Wire Transfer",
            "Cryptocurrency",
            "Mobile Payment",
            "Direct Debit"
        }));
    }
}
