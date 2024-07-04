using Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
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

        private string GetUserId()
        {
            return HttpContext.User.FindFirst("userId")!.Value;
        }
    }
}
