using Budget.Core.Models.Authentication;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<TokenModel> LoginAsync(LoginModel loginModel);

        Task RegisterAsync(RegisterModel registerModel);
    }
}
