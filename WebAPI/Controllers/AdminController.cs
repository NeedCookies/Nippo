using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public class AdminController(IUserService userService): ControllerBase
    {
        [HttpPost("give-points")]
        public async Task<IActionResult> GivePointsToUser(GivePointsRequest givePointsRequest)
        {
            var newUserInfo = await userService.GivePointsToUser(givePointsRequest.UserId, givePointsRequest.Points);

            return Ok(newUserInfo);
        }

        [HttpGet("get-users-roles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var usersAndRoles = await userService.GetUsersAndRoles(); 
            return Ok(usersAndRoles);
        }

        [HttpPost("generate-new-users")]
        public async Task<IActionResult> GenerateUsers()
        {
            var users = await userService.GenerateUsers();
            return Ok(users);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(string userId, string roleId)
        {
            await userService.AssignRole(userId, roleId);
            var updatedUser = await userService.GetUserInfoById(userId);
            return Ok(updatedUser);
        }


    }
}
