using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Admin;
using Budget.Domain.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Budget.Domain.Constants.ValidationMessages;

namespace Budget.Application.Services
{
    internal class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userModels = new List<UserModel>();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var userModel = UserModel.FromApplicationUser(user, userRoles);

                userModels.Add(userModel);
            }

            return userModels;
        }

        public async Task<bool> DeleteUserAsync(string userId, string currentUserId)
        {
            if (userId == currentUserId)
            {
                throw new BudgetValidationException(Admin.CannotDeleteYourAccount);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new BudgetAuthenticationException(Authentication.UserWithIdDoesNotExists, userId);
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> ChangeUserRoleAsync(ChangeUserRoleRequestModel changeUserRoleRequestModel, string currentUserId)
        {
            if (changeUserRoleRequestModel.UserId == currentUserId)
            {
                throw new BudgetValidationException(Admin.CannotChangeYourRole);
            }

            var user = await _userManager.FindByIdAsync(changeUserRoleRequestModel.UserId);
            if (user == null)
            {
                throw new BudgetAuthenticationException(Authentication.UserWithIdDoesNotExists, changeUserRoleRequestModel.UserId);
            }

            var allRoles = await _roleManager.Roles.ToListAsync();
            var newRole = allRoles.FirstOrDefault(r => r.Name == changeUserRoleRequestModel.RoleName);

            if (newRole != null)
            {
                foreach (var role in allRoles)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name!))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name!);
                    }
                }

                var result = await _userManager.AddToRoleAsync(user, newRole.Name!);
                return result.Succeeded;
            }

            return false;
        }
    }
}
