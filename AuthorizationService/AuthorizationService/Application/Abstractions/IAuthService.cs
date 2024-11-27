using AuthorizationService.Application.Contracts;
using AuthorizationService.Core;

namespace AuthorizationService.Application.Abstractions
{
    public interface IAuthService
    {
        Task RegisterUserAsync(
            string firstName, string lastName, DateOnly birthDate, string email, string password);
        Task<string> LoginUserAsync(string email, string password);
        Task<HashSet<Permission>> GetUserPermissionsAsync(string userId);
    }
}
