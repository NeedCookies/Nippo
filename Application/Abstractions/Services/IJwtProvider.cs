using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface IJwtProvider
    {
        string Generate(ApplicationUser user);
    }
}
