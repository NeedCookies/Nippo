using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class QuestionRepository(AppDbContext dbContext) : IQuestionRepository
    {
        public async Task<Question> Create(int order, int quizId, string Text, QuestionType type)
        {
            var question = new Question();
            question.Order = order;
            question.QuizId = quizId;
            question.Text = Text;
            question.Type = type;

            await dbContext.Questions.AddAsync(question);
            await dbContext.SaveChangesAsync();

            return question;
        }

        public async Task<Question> Delete(int questionId)
        {
            var question = await dbContext.Questions.Where(q => q.Id == questionId).FirstOrDefaultAsync();

            if (question == null)
            {
                throw new NullReferenceException($"No question with id:{questionId}");
            }

            dbContext.Questions.Remove(question);
            await dbContext.SaveChangesAsync();

            return question;
        }

        public async Task<Question> GetById(int questionId)
        {
            var question =  await dbContext.Questions.
                Where(q => q.Id == questionId).
                FirstOrDefaultAsync();

            if (question == null)
            {
                throw new NullReferenceException($"No question with id:{ questionId }");
            }

            return question;
        }

        public async Task<List<Question>> GetByQuiz(int quizId)
        {
            return await dbContext.Questions.
                Where(q => q.QuizId == quizId).
                ToListAsync();
        }
    }
}
