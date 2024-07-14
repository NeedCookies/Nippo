using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Update;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAnswerController(IUserAnswerService userAnswerService) 
        : ControllerBase
    {
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById(int userAnswerId)
        {
            var userAnswer = await userAnswerService.GetById(userAnswerId);
            return Ok(userAnswer);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserAnswerRequest request)
        {
            var userAnswer = await userAnswerService.Update(request);
            return Ok(userAnswer);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveUserAnswerAsync([FromBody] CreateUserAnswerRequest request)
        {
            var userId = GetUserId();
            var userAnswer = await userAnswerService.SaveUserAnswerAsync(request, userId);
            return Ok(userAnswer);
        }
        private string GetUserId()
        {
            return HttpContext.User.FindFirst("userId")!.Value;
        }
    }
}
