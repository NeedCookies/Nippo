using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizController(IQuizService quizService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetByCourse(int courseId)
        {
            var quizes = await quizService.GetByCourseId(courseId);
            return Ok(quizes);
        }

        [HttpGet("get-quiz")]
        public async Task<IActionResult> GetById(int quizId)
        {
            var quiz = await quizService.GetById(quizId);
            return Ok(quiz);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateQuizRequest request)
        {
            var quiz = await quizService.Create(request);
            return Ok(quiz);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int quizId)
        {
            var quiz = await quizService.Delete(quizId);
            return Ok(quiz);
        }
    }
}