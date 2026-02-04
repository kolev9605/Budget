using Microsoft.AspNetCore.Identity;

namespace Budget.Api.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Account> Accounts { get; set; } =  new List<Account>();

    public ICollection<UserCategory> Categories { get; set; } = new List<UserCategory>();
}
