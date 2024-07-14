using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class QuizResultRepository(AppDbContext dbContext)
        : IQuizResultRepository
    {
        public async Task<QuizResult> Create(int quizId, string userId, int score, int attempt)
        {
            var quizResult = new QuizResult
            {
                QuizId = quizId,
                UserId = userId,
                Score = score,
                Attempt = attempt
            };

            await dbContext.QuizResults.AddAsync(quizResult);
            await dbContext.SaveChangesAsync();

            return quizResult;
        }

        public async Task<QuizResult?> GetQuizResultByQuizAsync(int quizId, string userId)
        {
            return await dbContext.QuizResults
                .FirstOrDefaultAsync(qr => qr.QuizId == quizId
                && qr.UserId == userId);
        }
    }
}
