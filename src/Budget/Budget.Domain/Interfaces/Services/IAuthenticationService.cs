using Budget.Domain.Models.Authentication;
using System.Threading.Tasks;

namespace Budget.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<TokenModel> LoginAsync(LoginModel loginModel);

        Task<RegistrationResultModel> RegisterAsync(RegisterModel registerModel);
    }
}
