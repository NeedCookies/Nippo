using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface IJwtProvider
    {
        Task<string> GetUserId(string token);
        Task<string> GenerateAsync(ApplicationUser user);
    }
}
