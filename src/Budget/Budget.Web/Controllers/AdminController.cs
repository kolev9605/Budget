﻿using Budget.Application.Interfaces.Services;
using Budget.Application.Models.Admin;
using Budget.Application.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Budget.Web.Controllers
{
    [Authorize(Roles = Roles.Administrator)]
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route(nameof(GetUsers))]
        public async Task<IActionResult> GetUsers()
            => Ok(await _userService.GetUsersAsync());

        [HttpGet]
        [Route(nameof(DeleteUser))]
        public async Task<IActionResult> DeleteUser(string userId)
            => Ok(await _userService.DeleteUserAsync(userId, LoggedInUserId));

        [HttpPost]
        [Route(nameof(ChangeUserRole))]
        public async Task<IActionResult> ChangeUserRole(ChangeUserRoleRequestModel changeUserRoleRequestModel)
            => Ok(await _userService.ChangeUserRoleAsync(changeUserRoleRequestModel, LoggedInUserId));
    }
}
