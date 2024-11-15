using AuthorizationService.Core;

namespace AuthorizationService.Application.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}