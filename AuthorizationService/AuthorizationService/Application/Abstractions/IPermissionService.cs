using AuthorizationService.Core;

namespace AuthorizationService.Application.Abstractions
{
    public interface IPermissionService
    {
        Task<HashSet<Permission>> GetPermissionsAsync(Guid userId);
    }
}
