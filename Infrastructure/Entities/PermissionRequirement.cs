using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Entities
{
    /// <summary>
    /// Class for storing array of permissions
    /// </summary>
    public class PermissionRequirement(
        Permission permissions) : IAuthorizationRequirement
    {
        public Permission Permission { get; set; } = permissions;
    }
}
