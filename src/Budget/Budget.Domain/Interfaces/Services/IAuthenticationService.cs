using Budget.Domain.Models.Authentication;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(LoginModel loginModel);

        Task<RegistrationResult> RegisterAsync(RegisterModel registerModel);
    }
}
