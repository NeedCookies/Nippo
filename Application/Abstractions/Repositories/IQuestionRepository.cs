using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetByQuiz(int quizId);
        Task<Question> GetById(int questionId);
        Task<Question> Create(int order, int quizId, string Text, QuestionType type);
        Task<Question> Delete(int questionId);
    }
}
