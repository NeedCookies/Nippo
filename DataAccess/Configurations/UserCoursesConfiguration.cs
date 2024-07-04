using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class UserCoursesConfiguration : IEntityTypeConfiguration<UserCourses>
    {
        public void Configure(EntityTypeBuilder<UserCourses> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(uc => uc.User)
                            .WithMany(u => u.UserCourses)
                            .HasForeignKey(uc => uc.UserId)
                            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.Course)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
