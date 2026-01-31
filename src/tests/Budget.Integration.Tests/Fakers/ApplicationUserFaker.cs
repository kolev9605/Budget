using Bogus;
using Budget.Domain.Entities;

namespace Budget.Integration.Tests.Fakers;

public class ApplicationUserFaker : Faker<ApplicationUser>
{
    public ApplicationUserFaker()
    {
        RuleFor(u => u.Id, f => Guid.NewGuid().ToString());
        RuleFor(u => u.UserName, f => f.Internet.UserName());
        RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.UserName));
        RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());
    }
}
