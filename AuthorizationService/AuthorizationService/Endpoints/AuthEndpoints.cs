using Microsoft.AspNetCore.Mvc;
using AuthorizationService.Application.Abstractions;
using AuthorizationService.Application.Contracts;
using System.Security.Authentication;

namespace AuthorizationService.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("get-user-permissions", GetUserPermissions);
            app.MapPost("logout", Logout);
            return app;
        }

        private static async Task<IResult> Register(
            UserRegisterRequest request, 
            IAuthService authService)
        {
            try
            {
                await authService.RegisterUserAsync(
                request.firstName, request.lastName, request.birthDate, request.email, request.password);
                return Results.Ok();
            }
            catch (AuthenticationException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
        }

        private static async Task<IResult> Login(UserLoginRequest request, 
            IAuthService authService,
            HttpContext context)
        {
            try
            {
                var token = await authService.LoginUserAsync(request.email, request.password);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                context.Response.Cookies.Append("jwt-token-cookie", token, cookieOptions);

                return Results.Ok();
            }
            catch (AuthenticationException ex)
            {
                return Results.Conflict(new { message = ex.Message });
            }
            catch (NullReferenceException ex)
            {
                return Results.NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: 500);
            }
        }

        private static async Task<IResult> Logout(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete("jwt-token-cookie");

            return Results.Ok(new { message = "Logged out successfully" });
        }

        private static async Task<IResult> GetUserPermissions(
            IAuthService authService,
            [FromQuery] string userId)
        {
            var permissions =  await authService.GetUserPermissionsAsync(userId);
            return Results.Ok(permissions);
        }
    }
}
