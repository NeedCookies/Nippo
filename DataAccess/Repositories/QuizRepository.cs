using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class QuizRepository(AppDbContext dbContext) : IQuizRepository
    {
        public async Task<Quiz> Create(int courseId, string title, int order)
        {
            var quiz = new Quiz
            {
                CourseId = courseId,
                Title = title,
                Order = order
            };

            await dbContext.Quizzes.AddAsync(quiz);
            await dbContext.SaveChangesAsync();

            return quiz;
        }

        public async Task<Quiz> Delete(int quizId)
        {
            var quiz = await dbContext.Quizzes.
                Where(quiz => quiz.Id == quizId).
                FirstOrDefaultAsync();

            if (quiz == null)
            {
                throw new NullReferenceException($"No quiz with id:{quizId}");
            }

            dbContext.Quizzes.Remove(quiz);
            await dbContext.SaveChangesAsync();
            return quiz;
        }

        public async Task<List<Quiz>> GetQuizzesByCourseAsync(int courseId)
        {
            return await dbContext.Quizzes.
                Where(quiz => quiz.CourseId == courseId).
                ToListAsync();
        }

        public async Task<Quiz> GetQuizByIdAsync(int quizId)
        {
            var quiz = await dbContext.Quizzes.
                Where(quiz => quiz.Id == quizId).
                FirstOrDefaultAsync();

            if (quiz == null)
            {
                throw new NullReferenceException($"No quiz with id:{quizId}");
            }
            
            return quiz;
        }

        public async Task<int> GetQuizzesSizeByCourse(int courseId) =>
            await dbContext.Quizzes
                .Where(q => q.CourseId == courseId)
                .CountAsync();
    }
}
