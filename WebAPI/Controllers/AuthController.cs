﻿using Application.Abstractions.Services;
using Application.Contracts;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            HttpContext.Response.Cookies.Append("jwt-token-cookie", token, cookieOptions);

            return Ok(token);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt-token-cookie");

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
