using AuthorizationService.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;

namespace AuthorizationService.Persistance
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        


        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id).IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.FirstName).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<User>()
                .Property(u => u.LastName).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<User>()
                .Property(u => u.Email).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash).IsRequired().HasMaxLength(1024);
            modelBuilder.Entity<User>()
                .Property(u => u.BirthDate).IsRequired();
        }
        #endregion
    }
}
