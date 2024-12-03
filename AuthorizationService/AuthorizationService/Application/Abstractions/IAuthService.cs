using AuthorizationService.Application.Contracts;
using AuthorizationService.Core;

namespace AuthorizationService.Application.Abstractions
{
    public interface IAuthService
    {
        Task RegisterUserAsync(
            string firstName, string lastName, DateOnly birthDate, string email, string password);
        Task<string> LoginUserAsync(string email, string password);
        /// <summary>
        /// Returns all permissions that user have
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<HashSet<string>> GetUserPermissionsAsync(string userId);
        /// <summary>
        /// Returns first role of the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetUserRoleAsync(string userId);
    }
}
