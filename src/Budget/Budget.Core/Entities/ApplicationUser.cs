using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Budget.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Account> Accounts { get; set; } =  new List<Account>();

        public ICollection<UserCategory> Categories { get; set; } = new List<UserCategory>();
    }
}
