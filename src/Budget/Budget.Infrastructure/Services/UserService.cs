using Budget.Core.Entities;
using Budget.Core.Exceptions;
using Budget.Core.Guards;
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
using static Budget.Core.Constants.ValidationMessages;

namespace Budget.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _jwtOptions;

        public UserService(
            UserManager<ApplicationUser> userManager, 
            IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
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
            };

            return tokenModel;
        }

        public async Task RegisterAsync(RegisterModel registerModel)
        {
            Guard.IsNotNullOrEmpty(registerModel.Username, nameof(registerModel.Username));
            Guard.IsNotNullOrEmpty(registerModel.Password, nameof(registerModel.Password));
            Guard.IsNotNullOrEmpty(registerModel.Email, nameof(registerModel.Email));

            var userExists = await _userManager.FindByNameAsync(registerModel.Username);
            if (userExists != null)
            {
                throw new BudgetAuthenticationException(Authentication.UserExists, registerModel.Username);
            }

            ApplicationUser user = new()
            {
                Email = registerModel.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerModel.Username,
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                throw new BudgetAuthenticationException(Authentication.RegisterFailed);
            }

            await _userManager.AddToRoleAsync(user, Roles.User);
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
