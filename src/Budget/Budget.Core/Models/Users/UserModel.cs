using Budget.Core.Entities;
using System.Collections.Generic;

namespace Budget.Core.Models.Users
{
    public class UserModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public static UserModel FromApplicationUser(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            return new UserModel
            {
                Id = applicationUser.Id,
                Username = applicationUser.UserName,
                Roles = new List<string>(roles)
            };
        }
    }
}
