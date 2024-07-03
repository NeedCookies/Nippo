using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface IJwtProvider
    {
        Task<string> Generate(ApplicationUser user);
    }
}
