using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class QuestionEntityConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Text).IsRequired();
            builder.HasOne(q => q.Quiz).WithMany(q => q.Questions);
            builder.HasMany(q => q.Answers).WithOne(a => a.Question);
        }
    }
}
