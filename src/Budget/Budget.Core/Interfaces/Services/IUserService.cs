using Budget.Core.Models.Authentication;
using Budget.Core.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<TokenModel> LoginAsync(LoginModel loginModel);

        Task<string> RegisterAsync(RegisterModel registerModel);

        Task<IEnumerable<UserModel>> GetUsersAsync();
    }
}
