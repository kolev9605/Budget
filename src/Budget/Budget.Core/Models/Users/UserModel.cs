using Budget.Core.Entities;
using System.Collections.Generic;

namespace Budget.Core.Models.Users
{
    public class UserModel
    {
        public string Username { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public static UserModel FromApplicationUser(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            return new UserModel
            {
                Username = applicationUser.UserName,
                Roles = new List<string>(roles)
            };
        }
    }
}
