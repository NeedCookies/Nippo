using Application.Abstractions.Services;
using Application.Contracts;
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

        [HttpPost("get-courses-to-check")]
        public async Task<IActionResult> GetCoursesToCheck()
        {
            var courses = await coursesService.GetCoursesToCheck();

            return Ok(courses);
        }

        [HttpPost("accept-course")]
        public async Task<IActionResult> AcceptCourse(int courseId)
        {
            await publishEndpoint.Publish(new NotificationEvent()
            {
                Recipient = "lmo2004@bk.ru",
                Subject = "Результат проверки курса",
                Body = "Курс был принят и размещен на платформе",
                Type = NotificationType.Email
            });

            return Ok(await coursesService.AcceptCourse(courseId));
        }

        [HttpPost("cancel-course")]
        public async Task<IActionResult> CancelCourse(int courseId)
        {
            return Ok(await coursesService.CancelCourse(courseId));
        }

        [HttpPost("add-promocode")]
        public async Task<IActionResult> AddPromocode(PromocodeDto promocodeDto)
        {
            await discountService.AddPromocode(promocodeDto);

            return Ok();
        }
    }
}
