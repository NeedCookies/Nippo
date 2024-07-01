using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class AnswerRepository(AppDbContext dbContext) : IAnswerRepository
    {
        public async Task<Answer> Create(int QuestionId, string Text, bool isCorrect)
        {
            var answer = new Answer();
            answer.QuestionId = QuestionId;
            answer.Text = Text;
            answer.IsCorrect = isCorrect;

            await dbContext.AddAsync(answer);
            await dbContext.SaveChangesAsync();

            return answer;
        }

        public async Task<Answer> Delete(int answerId)
        {
            var answer = await dbContext.Answers.Where(a => a.Id == answerId).FirstOrDefaultAsync();

            if (answer == null)
                throw new NullReferenceException($"No answer with id: {answerId}");

            dbContext.Answers.Remove(answer);
            await dbContext.SaveChangesAsync();

            return answer;
        }

        public async Task<Answer> GetById(int answerId)
        {
            var answer = await dbContext.Answers.Where(a => a.Id == answerId).FirstOrDefaultAsync();

            if (answer == null)
                throw new NullReferenceException($"No answer with id: {answerId}");

            return answer;
        }

        public async Task<List<Answer>> GetByQuestion(int questionId)
        {
            return await dbContext.Answers.Where(a => a.QuestionId == questionId).ToListAsync();
        }
    }
}
