using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IAnswerRepository
    {
        Task<List<Answer>> GetByQuestion(int questionId);
        Task<Answer> GetById(int answerId);
        Task<Answer> Create(int questionId, string text, bool isCorrect);
        Task<Answer> Delete(int answerId);
        Task<Answer> Update(int answerId, string text, bool isCorrect);
    }
}
