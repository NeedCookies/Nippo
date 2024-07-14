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

        [Authorize(Roles = "author")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateLessonRequest request)
        {
            var lesson = await lessonsService.Create(request);

            return Ok(lesson);
        }

        [Authorize(Roles = "author")]
        [HttpPost("update")]
        public async Task<IActionResult> Update(int lessonId, string title)
        {
            var lesson = await lessonsService.Update(lessonId, title);

            return Ok(lesson);
        }

        [Authorize(Roles = "author")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int lessonId)
        {
            var lesson = await lessonsService.Delete(lessonId);

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
