namespace Application.Abstractions.Services
{
    public interface IJwtProvider
    {
        Task<string> GetUserId(string token);
    }
}
