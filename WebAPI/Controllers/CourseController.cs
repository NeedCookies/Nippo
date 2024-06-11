using Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("course")]
    public class CourseController(ICoursesService coursesService): ControllerBase
    {
        [HttpGet("get-all-courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var allCourses = await coursesService.GetAllCourses();

            return Ok(allCourses);
        }
    }
}
