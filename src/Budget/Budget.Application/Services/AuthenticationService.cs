using Budget.Domain.Entities;
using Budget.Domain.Exceptions;
using Budget.Domain.Guards;
using Budget.Domain.Interfaces;
using Budget.Domain.Interfaces.Repositories;
using Budget.Domain.Interfaces.Services;
using Budget.Domain.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Budget.Domain.Constants.ValidationMessages.Authentication;

namespace Budget.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ICategoryRepository _categoryRepository;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            ICategoryRepository categoryRepository)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _categoryRepository = categoryRepository;
        }

        public async Task<AuthenticationResult> LoginAsync(LoginModel loginModel)
        {
            Guard.IsNotNullOrEmpty(loginModel.Username, nameof(loginModel.Username));
            Guard.IsNotNullOrEmpty(loginModel.Password, nameof(loginModel.Password));

            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user == null)
            {
                throw new BudgetAuthenticationException(UserDoesNotExists, loginModel.Username);
            }

            if (!await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                throw new BudgetAuthenticationException(IncorrectPassword);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            (var token, var validTo) = _jwtTokenGenerator.GenerateToken(userRoles, user.Id, user.Email!);

            // var tokenModel = new AuthenticationResult(default, default, default)
            // {
            //     Token = token,
            //     ValidTo = validTo,
            //     Roles = userRoles,
            // };

            return null;
        }

        public async Task<RegistrationResult> RegisterAsync(RegisterModel registerModel)
        {
            Guard.IsNotNullOrEmpty(registerModel.Username, nameof(registerModel.Username));
            Guard.IsNotNullOrEmpty(registerModel.Password, nameof(registerModel.Password));
            Guard.IsNotNullOrEmpty(registerModel.Email, nameof(registerModel.Email));

            var userExists = await _userManager.FindByNameAsync(registerModel.Username);
            if (userExists != null)
            {
                throw new BudgetAuthenticationException(UserExists, registerModel.Username);
            }

            var initialCategories = await _categoryRepository.GetInitialCategoriesAsync();
            var userCategories = initialCategories
                .Select(c => new UserCategory()
                {
                    Category = c,
                }).ToList();

            ApplicationUser user = new()
            {
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.Username,
                Categories = userCategories
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                throw new BudgetAuthenticationException(RegisterFailed);
            }

            await _userManager.AddToRoleAsync(user, Roles.User);
            return new RegistrationResult(user.Id);
        }
    }
}
