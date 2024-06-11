using Application.Abstractions.Repositories;
using Domain.Entities;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    public class AnswerRepository(AppDbContext dbContext) : IAnswerRepository
    {
        public async Task<List<Answer>> GetAnswersByQuestion(int questionId)
        {
            return await dbContext.Answers.Where(a => a.QuestionId == questionId).ToListAsync();
        }
    }
}
