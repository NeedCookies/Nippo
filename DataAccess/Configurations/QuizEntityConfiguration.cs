using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DataAccess.Configurations
{
    public class QuizEntityConfiguration: IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Title).HasMaxLength(255).IsRequired();

            builder.HasOne(q => q.Course)
                .WithMany(c => c.Quizes)
                .HasForeignKey(q => q.CourseId);

            //builder.HasMany(q => q.UserProgresses)
            //    .WithOne()
            //    .HasForeignKey(up => up.ElementId);
        }
    }
}
