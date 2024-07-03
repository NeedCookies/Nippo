using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface IJwtProvider
    {
        Task<string> Generate(ApplicationUser user);
        Task<string> GetUserId(string token);
    }
}
