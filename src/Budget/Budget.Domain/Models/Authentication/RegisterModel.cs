using System.ComponentModel.DataAnnotations;

namespace Budget.Domain.Models.Authentication;

public class RegisterModel
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;
}
