using AuthorizationService.Core;
using AuthorizationService.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationService.Persistance.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(u => u.FirstName)
                .IsRequired().HasMaxLength(128);
            builder.Property(u => u.LastName)
                .HasMaxLength(128);
            builder.Property(u => u.Email)
                .IsRequired().HasMaxLength(256);
            builder.Property(x => x.PasswordHash)
                .IsRequired().HasMaxLength(512);

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRoleEntity>(
                l => l.HasOne<RoleEntity>().WithMany().HasForeignKey(r => r.RoleId),
                r => r.HasOne<UserEntity>().WithMany().HasForeignKey(l => l.UserId));
        }
    }
}
