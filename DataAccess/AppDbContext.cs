using DataAccess.Configurations;
using Domain.Entities;
using Domain.Entities.Identity;
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
        public DbSet<QuizResult> QuizResults => Set<QuizResult>();
        public DbSet<UserAnswer> UserAnswers => Set<UserAnswer>();
        public DbSet<UserCourses> UserCourses => Set<UserCourses>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AnswerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BlockEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CourseEntityConfiguration());
            modelBuilder.ApplyConfiguration(new LessonEntityConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new QuizEntityConfiguration());
            modelBuilder.ApplyConfiguration(new QuizResultEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserAnswerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserCoursesConfiguration());

            //SeedRoles(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        /*
        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<AppRole>().HasData(
                new AppRole("user") { Id = "1" },
                new AppRole("author") { Id = "2" },
                new AppRole("admin") { Id = "3" }
            );
        }
        */
    }
}
