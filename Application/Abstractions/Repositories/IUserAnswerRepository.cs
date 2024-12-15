using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserAnswerRepository
    {
        Task<UserAnswer?> GetById(int userAnswerId);
        Task<UserAnswer> Create(int questionId, string text, Guid userId, int attempt);
        Task<UserAnswer> Delete(int userAnswerId);
        Task<UserAnswer?> GetByQuestion(Guid userId, int questionId);
    }
}
