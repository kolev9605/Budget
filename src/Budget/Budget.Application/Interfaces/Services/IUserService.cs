using Budget.Application.Models.Admin;
using Budget.Application.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetUsersAsync();

        Task<bool> DeleteUserAsync(string userId, string currentUserId);

        Task<bool> ChangeUserRoleAsync(ChangeUserRoleRequestModel changeUserRoleRequestModel, string currentUserId);
    }
}
