using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface IJwtProvider
    {
        Task<string> GenerateAsync(ApplicationUser user);
    }
}
