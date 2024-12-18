using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class CourseEntityConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title).HasMaxLength(255).IsRequired();
            builder.Property(c => c.Description).IsRequired();
            builder.Property(c => c.Status).IsRequired();
            builder.Property(c => c.Price).IsRequired();

            builder.HasOne(c => c.Author)
                .WithMany(a => a.Courses)
                .HasForeignKey(c => c.AuthorId);
        }
    }
}
