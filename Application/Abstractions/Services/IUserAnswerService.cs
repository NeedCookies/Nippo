using Application.Contracts;
using Application.Contracts.Update;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface IUserAnswerService
    {
        Task<UserAnswer> GetById(int userAnswerId);
        Task<UserAnswer> Update(UpdateUserAnswerRequest request);
        Task<UserAnswer> Delete(int userAnswerId);
        Task<UserAnswer> SaveUserAnswerAsync(CreateUserAnswerRequest request, string userId);
    }
}
