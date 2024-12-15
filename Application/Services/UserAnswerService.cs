using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Update;
using Domain.Entities;

namespace Application.Services
{
    public class UserAnswerService(IUserAnswerRepository userAnswerRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IQuestionRepository questionRepository)
        : IUserAnswerService
    { 

        public async Task<UserAnswer> Delete(int userAnswerId)
        {
            var userAnswer = GetById(userAnswerId);

            if (userAnswer == null)
                throw new ArgumentException($"No user answer with Id{userAnswerId}");

            return await userAnswerRepository.Delete(userAnswerId);
        }

        public async Task<UserAnswer> GetById(int userAnswerId)
        {
            var userAnswer = await userAnswerRepository.GetById(userAnswerId);

            if (userAnswer == null)
                throw new ArgumentException($"No User Answer with id {userAnswerId}");

            return userAnswer;
        }

        public async Task<UserAnswer> SaveUserAnswerAsync(CreateUserAnswerRequest request, string userId)
        {
            int questionId = request.QuestionId;
            string text = request.Text.Trim();

            var userAnswer = await userAnswerRepository.GetByQuestion(userId, request.QuestionId);

            if (userAnswer == null)
            {
                // create user answer
                int attempt = 1;
                return await
                userAnswerRepository.Create(questionId, text, userId, attempt);
            }
            else
            {
                //update
                userAnswer.Attempt++;
                userAnswer.Text = text;

                await unitOfWork.SaveChangesAsync();
                return userAnswer;
            }
        }

        public async Task<UserAnswer> Update(UpdateUserAnswerRequest request)
        {
            int userAnswerId = request.UserAnswerId;
            string text = request.Text.Trim();

            var userAnswer = await GetById(userAnswerId);

            if (userAnswer == null)
                throw new ArgumentException($"No user answer with id {userAnswerId}");

            if (text == null || text.Length == 0)
                throw new ArgumentException($"Answer text shouldn't bew empty");

            userAnswer.Text = text;
            userAnswer.Attempt += 1;

            await unitOfWork.SaveChangesAsync();

            return await GetById(userAnswerId);
        }
    }
}
