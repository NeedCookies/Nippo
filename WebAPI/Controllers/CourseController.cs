using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("course")]
    public class CourseController(ICoursesService coursesService): ControllerBase
    {
        [HttpGet("get-all-courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var allCourses = await coursesService.GetAllCourses();

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

        [HttpPost("create-course")]
        public async Task<IActionResult> Create([FromBody] CreateCourseRequest request)
        {
            var course = await coursesService.Create(request);
            return Ok(course);
        }

        [HttpGet("test-query")]
        public async Task<IActionResult> TestQuery() => Ok("Aboba");
    }
}
