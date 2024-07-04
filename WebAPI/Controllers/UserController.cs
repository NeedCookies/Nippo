using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Authorize]
    [Route("user")]
    public class UserController(IUserService userService, IJwtProvider jwtProvider) : ControllerBase
    {
        [HttpPost("get-courses")]
        public async Task<IActionResult> GetUserCourses()
        {
            string token = HttpContext.Request.Cookies["jwt-token-cookie"];
            string userId = await jwtProvider.GetUserId(token);
            if(userId == null)
            {
                throw new Exception("userId is null");
            }
            var courses = await userService.GetUserCourses(userId);
            return Ok(courses);
        }

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
