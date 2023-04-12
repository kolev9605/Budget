using Budget.Application.Models.Authentication;
using System.Threading.Tasks;

namespace Budget.Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<TokenModel> LoginAsync(LoginModel loginModel);

        Task<RegistrationResultModel> RegisterAsync(RegisterModel registerModel);
    }
}
