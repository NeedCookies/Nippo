using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserAnswerRepository(AppDbContext dbContext)
        : IUserAnswerRepository
    {
        public async Task<UserAnswer> Create(int questionId, string text, string userId, int attempt)
        {
            var userAnswer = new UserAnswer
            {
                QuestionId = questionId,
                Text = text,
                UserId = userId,
                Attempt = attempt
            };

            await dbContext.UserAnswers.AddAsync(userAnswer);
            await dbContext.SaveChangesAsync();
            
            return userAnswer;
        }

        public async Task<UserAnswer> Delete(int userAnswerId)
        {
            var userAnswer = await dbContext.UserAnswers
                .FirstAsync(ua => ua.Id == userAnswerId);

            dbContext.UserAnswers.Remove(userAnswer);
            await dbContext.SaveChangesAsync();

            return userAnswer;
        }

        public async Task<UserAnswer?> GetById(int userAnswerId)
        {
            return await dbContext.UserAnswers
                .FirstOrDefaultAsync(ua => ua.Id == userAnswerId);
        }

        public async Task<UserAnswer?> GetByQuestion(string userId, int questionId)
        {
            return await dbContext.UserAnswers
                .FirstOrDefaultAsync(ua => ua.UserId == userId
                && ua.QuestionId == questionId);
        }
    }
}
