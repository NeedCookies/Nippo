using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("course")]
    public class CourseController(ICoursesService coursesService, IJwtProvider jwtProvider, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("get-all-courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            //var allCourses = await coursesService.GetAllCourses();
            var allCourses = "all courses";
            return Ok(allCourses);
        }

        [HttpGet("get-course")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await coursesService.GetById(id);
            
            if (course == null)
            {
                return BadRequest("No such id");
            }

            return Ok(course);
        }

        [Authorize(Policy = "CreateCourse")]
        [HttpPost("create-course")]
        public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
        {
            string token = HttpContext.Request.Cookies["jwt-token-cookie"];
            string userId = await jwtProvider.GetUserId(token);

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with id '{userId}' not found.");
                // Или выполняем другие действия по обработке ошибки, например, возвращаем NotFound или BadRequest
            }

            var course = await coursesService.Create(request, userId);
            return Ok(course);
        }

        [Authorize]
        [HttpPost("purchase-course")]
        public async Task<IActionResult> PurchaseCourse(int courseId)
        {
            string token = HttpContext.Request.Cookies["jwt-token-cookie"];
            string userId = await jwtProvider.GetUserId(token);

            var result = await coursesService.PurchaseCourse(courseId, userId);
            return Ok(result);
        }

        [Authorize(Policy = "UpdateCourse")]
        [HttpPost("edit-course")]
        public async Task<IActionResult> Update([FromBody] UpdateCourseRequest request)
        {
            var course = await coursesService.Update(request);
            return Ok(course);
        }

        [Authorize(Policy = "DeleteCourse")]
        [HttpDelete("delete-course")]
        public async Task<IActionResult> Delete(int courseId)
        {
            var course = await coursesService.Delete(courseId);
            return Ok(course);
        }


        [HttpGet("test-query")]
        public async Task<IActionResult> TestQuery() => Ok("Aboba");
    }
}
