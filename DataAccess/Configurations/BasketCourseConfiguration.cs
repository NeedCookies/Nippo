using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    /// <summary>
    /// Represents course which user added to basket
    /// Middle table in many to many relationshib
    /// between users and their courses in basket
    /// </summary>
    public class BasketCourseConfiguration :
        IEntityTypeConfiguration<BasketCourses>
    {
        public void Configure(EntityTypeBuilder<BasketCourses> builder)
        {
            builder.HasKey(bc => bc.Id);

            builder.HasOne(bc => bc.User)
                .WithMany(u => u.BasketCourses)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bc => bc.Course)
                .WithMany(c => c.BasketCourses)
                .HasForeignKey(c => c.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
