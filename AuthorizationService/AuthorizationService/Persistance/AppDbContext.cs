using AuthorizationService.Core;
using AuthorizationService.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;

namespace AuthorizationService.Persistance
{
    public class AppDbContext(DbContextOptions<AppDbContext> options,
            IOptions<AuthorizationOptions> authOptions) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));

            //base.OnModelCreating(modelBuilder);
        }
    }
}
