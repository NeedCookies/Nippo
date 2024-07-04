using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("user")]
    public class UserController (IUserService userService): ControllerBase
    {
        [HttpGet("get-personal-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = GetUserId();
            var userInfo = await userService.GetUserInfoById(userId);

            return Ok(userInfo);
        }

        [HttpPost("update-personal-info")]
        public async Task<IActionResult> UpdateUserInfo(
            [FromForm] UserInfoUpdateRequest infoUpdateRequest,
            IFormFile? userPicture)
        {
            var userId = GetUserId();

            var newUserInfo = await userService.UpdateUserInfo(userId, infoUpdateRequest, null);

            return Ok(newUserInfo);
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirst("userId")!.Value;
        }
    }
}
