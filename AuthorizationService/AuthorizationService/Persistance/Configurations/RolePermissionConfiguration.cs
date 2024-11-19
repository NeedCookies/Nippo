using AuthorizationService.Core;
using AuthorizationService.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Persistance.Configurations
{
    public class RolePermissionConfiguration
        (AuthorizationOptions authOptions)
        : IEntityTypeConfiguration<RolePermissionEntity>
    {
        private readonly AuthorizationOptions _authOptions = authOptions;

        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            // Составной ключ
            builder.HasKey(r => new { r.RoleId, r.PermissionId });

            builder.HasData(ParseRolePermissions());
        }

        public RolePermissionEntity[] ParseRolePermissions()
        {
            return _authOptions.RolePermissions
                .SelectMany(rp => rp.Permissions
                .Select(p => new RolePermissionEntity
                {
                    RoleId = (int)Enum.Parse<Role>(rp.Role),
                    PermissionId = (int)Enum.Parse<Permission>(p)
                }))
                .ToArray();
        }
    }
}
