using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Answer> Answers => Set<Answer>();
        public DbSet<Block> Blocks => Set<Block>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<QuizResult> QuizResult => Set<QuizResult>();
        public DbSet<UserAnswer> UserAnswers => Set<UserAnswer>();
    }
}
