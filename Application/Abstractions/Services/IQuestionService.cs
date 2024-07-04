using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface IQuestionService
    {
        Task<List<Question>> GetByQuizId(int quizId);
        Task<Question> GetById(int questionId);
        Task<Question> Create(CreateQuestionRequest request);
        Task<Question> Delete(int questionId);
        Task<Question> Update(int questionId, string text);
    }
}
