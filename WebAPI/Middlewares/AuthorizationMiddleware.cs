namespace WebAPI.Middlewares
{
    public class AuthorizationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("jwt-token-cookie", out string token))
            {
                context.Request.Headers.Authorization = $"Bearer {token}";
            }

            await _next.Invoke(context);
        }
    }
}
