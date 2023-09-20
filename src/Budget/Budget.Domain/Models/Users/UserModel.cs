using Budget.Domain.Entities;
using System.Collections.Generic;

namespace Budget.Domain.Models.Users;

public class UserModel
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public IEnumerable<string> Roles { get; set; } = new List<string>();

    public static UserModel FromApplicationUser(ApplicationUser applicationUser, IEnumerable<string> roles)
    {
        return new UserModel
        {
            Id = applicationUser.Id,
            Username = applicationUser.UserName!,
            Roles = new List<string>(roles)
        };
    }
}
