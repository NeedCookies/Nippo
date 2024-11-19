using AuthorizationService.Application.Abstractions;
using AuthorizationService.Core;

namespace AuthorizationService.Application.Services
{
    public class PermissionService(
        IUserRepository userRepository) : IPermissionService
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<HashSet<Permission>> GetPermissionsAsync(Guid userId)
        {
            return await _userRepository.GetUserPermissionsAsync(userId);
        }
    }
}
