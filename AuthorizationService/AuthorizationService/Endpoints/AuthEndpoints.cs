using AuthorizationService.Application.Abstractions;
using AuthorizationService.Application.Contracts;

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
            await authService.RegisterUserAsync(
                request.firstName, request.lastName, request.birthDate, request.email, request.password);
            return Results.Ok();
        }

        private static async Task<IResult> Login(UserLoginRequest request, IAuthService authService)
        {
            var token = await authService.LoginUserAsync(request.email, request.password);

            if (token == null)
            {
                throw new InvalidDataException("Token hasn't been created");
            }

            return Results.Ok(token);
        }
    }
}
