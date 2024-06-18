using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class LessonEntityConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Title).HasMaxLength(255).IsRequired();
            builder.Property(l => l.CreateDate).HasColumnType("date");

            builder.HasOne(l => l.Course).WithMany(c => c.Lessons);
            builder.HasMany(l => l.Blocks).WithOne(b => b.Lesson);
        }
    }
}
