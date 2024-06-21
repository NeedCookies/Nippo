using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class BlockEntityConfiguration: IEntityTypeConfiguration<Block>
    {
        public void Configure(EntityTypeBuilder<Block> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Type).IsRequired();
            builder.Property(b => b.Content).IsRequired();

            builder.HasOne(b => b.Lesson)
                .WithMany(l => l.Blocks)
                .HasForeignKey(b => b.LessonId);
        }
    }
}
