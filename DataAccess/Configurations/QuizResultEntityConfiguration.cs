using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class QuizResultEntityConfiguration : IEntityTypeConfiguration<QuizResult>
    {
        public void Configure(EntityTypeBuilder<QuizResult> builder)
        {
            builder.HasKey(qr => qr.Id);

            builder.HasOne(qr => qr.Quiz)
                .WithMany(q => q.QuizResults)
                .HasForeignKey(qr => qr.QuizId);

            builder.HasOne(qr => qr.User)
                .WithMany(u => u.QuizResults)
                .HasForeignKey(u => u.UserId);
        }
    }
}
