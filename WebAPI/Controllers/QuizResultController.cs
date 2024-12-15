using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizResultController (IQuizResultService quizResultService)
        : ControllerBase
    {
        [HttpGet("get-by-quiz")]
        public async Task<IActionResult> GetByQuiz(int quizId)
        {
            var userId = GetUserId();
            var quizResult = await quizResultService.GetQuizResult(userId, quizId);

            return Ok(quizResult);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody]CreateQuizResultRequest request)
        {
            var userId = GetUserId();
            var quizResult = await quizResultService.SaveQuizResult(request, userId);

            return Ok(quizResult);
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirst("userId")!.Value;
        }
    }
}
