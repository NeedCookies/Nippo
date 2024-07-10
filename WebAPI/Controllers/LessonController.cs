using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("lesson")]
    public class LessonController(ILessonsService lessonsService) : ControllerBase
    {
        [HttpGet("get-by-course")]
        public async Task<IActionResult> GetLessonsByCourse(int courseId)
        {
            var lessons = await lessonsService.GetByCourseId(courseId, GetUserId());

            return Ok(lessons);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("create-lesson")]
        public async Task<IActionResult> Create([FromBody] CreateLessonRequest request)
        {
            var lesson = await lessonsService.Create(request);

            return Ok(lesson);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            var lesson = await lessonsService.GetById(id, GetUserId());

            return Ok(lesson);
        }

        private string GetUserId() =>
            HttpContext.User.FindFirst("userId")!.Value;
    }
}
