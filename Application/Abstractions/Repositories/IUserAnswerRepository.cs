using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserAnswerRepository
    {
        Task<UserAnswer?> GetById(int userAnswerId);
        Task<UserAnswer> Create(int questionId, string text, string userId, int attempt);
        Task<UserAnswer> Delete(int userAnswerId);
        Task<UserAnswer?> GetByQuestion(string userId, int questionId);
    }
}
