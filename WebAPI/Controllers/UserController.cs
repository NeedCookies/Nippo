using Application.Abstractions.Services;
using Application.Services;
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
            var courses = await userService.GetUserCourses(userId);
            return Ok(courses);
        }
    }
}
