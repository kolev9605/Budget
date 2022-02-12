using Budget.Core.Models.Authentication;

namespace Budget.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<TokenModel> LoginAsync(LoginModel loginModel);

        Task<bool> RegisterAsync(RegisterModel registerModel);
    }
}
