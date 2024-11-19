using AuthorizationService.Core;
using AuthorizationService.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Persistance.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RolePermissionEntity>(
                l => l.HasOne<PermissionEntity>().WithMany().HasForeignKey(p => p.PermissionId),
                r => r.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId));

            var roles = Enum.GetValues<Role>()
                .Select(r => new RoleEntity
                {
                    Id = (int)r,
                    Name = r.ToString()
                });

            builder.HasData(roles);
        }
    }
}
