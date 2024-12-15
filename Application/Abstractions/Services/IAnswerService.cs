using Application.Contracts;
using Application.Contracts.Update;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface IAnswerService
    {
        Task<List<Answer>> GetByQuestion(int questionId);
        Task<Answer> GetById(int answerId);
        Task<Answer> Create(CreateAnswerRequest request);
        Task<Answer> Delete(int answerId);
        Task<Answer> Update(UpdateAnswerRequest request);
    }
}
