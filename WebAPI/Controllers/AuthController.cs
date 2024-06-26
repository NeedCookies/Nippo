using Application.Abstractions.Services;
using Application.Contracts;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var user = await userService.Register(request.UserName, request.Email, request.Password);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest request)
        {
            var token = await userService.Login(request.UserName, request.Password);

            HttpContext.Response.Cookies.Append("jwt-token-cookie", token);
            
            return Ok(token);
        }
    }
}
