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

            builder.HasOne(uc => uc.Course)
                .WithMany(c => c.UserProgresses)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.Lesson)
                .WithMany(c => c.UserProgresses)
                .HasForeignKey(uc => uc.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.Quiz)
                .WithMany(c => c.UserProgresses)
                .HasForeignKey(uc => uc.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
