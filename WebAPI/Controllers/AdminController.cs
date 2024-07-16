using Application.Abstractions.Services;
using Application.Contracts;
using DataAccess;
using Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public class AdminController(
        IUserService userService, 
        ICoursesService coursesService,
        IDiscountService discountService,
        IPublishEndpoint publishEndpoint) : ControllerBase
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

        [HttpGet("get-courses-to-check")]
        public async Task<IActionResult> GetCoursesToCheck()
        {
            var courses = await coursesService.GetCoursesToCheck();

            return Ok(courses);
        }

        [HttpPost("accept-course")]
        public async Task<IActionResult> AcceptCourse(CourseIdRequest request)
        {
            var moderatedCourseInfo = await coursesService.AcceptCourse(request.courseId);

            await publishEndpoint.Publish(new NotificationEvent()
            {
                Recipient = moderatedCourseInfo.AuthorEmail,
                Subject = "Результат проверки курса",
                Body = moderatedCourseInfo.AdminAnswer,
                Type = NotificationType.Email
            });

            return Ok();
        }

        [HttpPost("cancel-course")]
        public async Task<IActionResult> CancelCourse(CourseIdRequest request)
        {
            var moderatedCourseInfo = await coursesService.CancelCourse(request.courseId);

            await publishEndpoint.Publish(new NotificationEvent()
            {
                Recipient = moderatedCourseInfo.AuthorEmail,
                Subject = "Результат проверки курса",
                Body = moderatedCourseInfo.AdminAnswer,
                Type = NotificationType.Email
            });

            return Ok();
        }

        [HttpPost("add-promocode")]
        public async Task<IActionResult> AddPromocode([FromBody]PromocodeDto promocodeDto)
        {
            await discountService.AddPromocode(promocodeDto);

            return Ok();
        }
    }
}
