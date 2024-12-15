using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("get-courses")]
        public async Task<IActionResult> GetUserCourses()
        {
            string userId = GetUserId();

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
            [FromForm] UpdateUserInfoRequest infoUpdateRequest)
        {
            var userId = GetUserId();

            var newUserInfo = await userService.UpdateUserInfo(userId, infoUpdateRequest);

            return Ok(newUserInfo);
        }

        [HttpGet("get-user-course-progress")]
        public async Task<IActionResult> GetUserCourseProgress(int courseId)
        {
            var userId = GetUserId();
            var userProgress = await userService.GetUserProgresses(userId, courseId);

            return Ok(userProgress);
        }

        [Authorize(Roles ="author")]
        [HttpGet("get-created-courses")]
        public async Task<IActionResult> GetCreatedCourses()
        {
            var courses = await userService.GetCreatedCourses(GetUserId());

            return Ok(courses);
        }

        [Authorize(Roles = "author")]
        [HttpGet("get-users-by-course")]
        public async Task<IActionResult> GetUsersByCourse(int courseId)
        {
            var users = await userService.GetUsersByCourse(courseId);

            return Ok(users);
        }

        [HttpPost("upgrade-to-author")]
        public async Task<IActionResult> UpgradeRoleToAuthor()
        {
            await userService.UpgradeRoleToAuthor(GetUserId());
            return Ok();
        }

        [Authorize(Roles ="author")]
        [HttpPost("downgrade-to-user")]
        public async Task<IActionResult> DowngradeRoleToUser()
        {
            await userService.DowngradeRoleToUser(GetUserId());
            return Ok();
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirst("userId")!.Value;
        }
    }
}
