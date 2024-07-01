using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizController(IQuizService quizService): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetQuizesByCourse(int courseId)
        {
            var quizes = await quizService.GetByCourseId(courseId);
            return Ok(quizes);
        }

        [HttpGet("get-quiz")]
        public async Task<IActionResult> GetQuizById(int quizId)
        {
            var quiz = await quizService.GetById(quizId);
            return Ok(quiz);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
        {
            var quiz = quizService.Create(request);
            return Ok(quiz);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int quizId)
        {
            var quiz = quizService.Delete(quizId);
            return Ok(quiz);
        }
    }
}
