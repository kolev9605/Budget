using Budget.Core.Models.Admin;
using Budget.Core.Models.Authentication;
using Budget.Core.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<TokenModel> LoginAsync(LoginModel loginModel);

        Task<RegistrationResultModel> RegisterAsync(RegisterModel registerModel);

        Task<IEnumerable<UserModel>> GetUsersAsync();

        Task<bool> DeleteUserAsync(string userId, string currentUserId);

        Task<bool> ChangeUserRoleAsync(ChangeUserRoleRequestModel changeUserRoleRequestModel, string currentUserId);
    }
}
