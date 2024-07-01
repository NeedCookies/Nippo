using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnswerController(IAnswerService answerService) : ControllerBase
    {
        [HttpGet("get-by-question")]
        public async Task<IActionResult> GetByQuestion(int questionId)
        {
            var answers = await answerService.GetByQuestion(questionId);
            return Ok(answers);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById(int answerId)
        {
            var answer = await answerService.GetById(answerId);
            return Ok(answer);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAnswerRequest request)
        {
            var answer = await answerService.Create(request);
            return Ok(answer);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int answerId)
        {
            var answer = await answerService.Delete(answerId);
            return Ok(answer);
        }
    }
}
