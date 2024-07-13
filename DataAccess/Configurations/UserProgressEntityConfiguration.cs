using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DataAccess.Configurations
{
    public class UserProgressEntityConfiguration : IEntityTypeConfiguration<UserProgress>
    {
        public void Configure(EntityTypeBuilder<UserProgress> builder)
        {
            builder.HasOne(up => up.User)
            .WithMany(u => u.UserProgresses)
            .HasForeignKey(up => up.UserId);
        }
    }
}
