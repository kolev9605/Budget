using System.ComponentModel.DataAnnotations;

namespace Budget.Application.Models.Authentication
{
    public class LoginModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
