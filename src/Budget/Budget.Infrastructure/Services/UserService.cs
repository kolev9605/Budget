using Budget.Core.Entities;
using Budget.Core.Exceptions;
using Budget.Core.Guards;
using Budget.Core.Interfaces.Repositories;
using Budget.Core.Interfaces.Services;
using Budget.Core.Models.Authentication;
using Budget.Core.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using static Budget.Core.Constants.ValidationMessages;
using Budget.Core.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Budget.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly ICategoryRepository _categoryRepository;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtOptions> jwtOptions, 
            ICategoryRepository categoryRepository)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            _categoryRepository = categoryRepository;
        }

        public async Task<TokenModel> LoginAsync(LoginModel loginModel)
        {
            Guard.IsNotNullOrEmpty(loginModel.Username, nameof(loginModel.Username));
            Guard.IsNotNullOrEmpty(loginModel.Password, nameof(loginModel.Password));

            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user == null)
            {
                throw new BudgetAuthenticationException(Authentication.UserDoesNotExists, loginModel.Username);
            }

            if (!await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                throw new BudgetAuthenticationException(Authentication.IncorrectPassword);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            (var token, var validTo) = GenerateToken(authClaims);

            var tokenModel = new TokenModel()
            {
                Token = token,
                ValidTo = validTo,
                Roles = userRoles,
            };

            return tokenModel;
        }

        public async Task<string> RegisterAsync(RegisterModel registerModel)
        {
            Guard.IsNotNullOrEmpty(registerModel.Username, nameof(registerModel.Username));
            Guard.IsNotNullOrEmpty(registerModel.Password, nameof(registerModel.Password));
            Guard.IsNotNullOrEmpty(registerModel.Email, nameof(registerModel.Email));

            var userExists = await _userManager.FindByNameAsync(registerModel.Username);
            if (userExists != null)
            {
                throw new BudgetAuthenticationException(Authentication.UserExists, registerModel.Username);
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
                throw new BudgetAuthenticationException(Authentication.RegisterFailed);
            }

            await _userManager.AddToRoleAsync(user, Roles.User);
            return user.Id;
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

        private (string token, DateTime validTo) GenerateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }
    }
}
