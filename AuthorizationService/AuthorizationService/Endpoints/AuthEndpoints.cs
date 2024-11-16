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
            return app;
        }

        private static async Task<IResult> Register(UserRegisterRequest request, IAuthService authService)
        {
            try
            {
                await authService.RegisterUserAsync(
                request.firstName, request.lastName, request.birthDate, request.email, request.password);
                return Results.Ok();
            }
            catch (AuthenticationException ex)
            {
                return Results.Conflict(new {message = ex.Message});
            }
        }

        private static async Task<IResult> Login(UserLoginRequest request, IAuthService authService,
            HttpContext context)
        {
            try
            {
                var token = await authService.LoginUserAsync(request.email, request.password);

                context.Response.Cookies.Append("bearer", token);

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
    }
}
