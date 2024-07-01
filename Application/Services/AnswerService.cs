using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using System.Text;

namespace Application.Services
{
    public class AnswerService(IAnswerRepository answerRepository) : IAnswerService
    {
        public async Task<Answer> Create(CreateAnswerRequest request)
        {
            int questionId = request.QuestionId;
            string text = request.Text;
            bool isCorrect = request.isCorrect;

            StringBuilder error = new StringBuilder();
            if (questionId < 0) error.AppendLine("Wrong question Id");
            if (text == null || text.Length == 0) error.AppendLine("Answer text shouldn't be empty");
            
            if (error.Length > 0) 
                throw new ArgumentException(error.ToString());

            return await answerRepository.Create(questionId, text, isCorrect);
        }

        public async Task<Answer> Delete(int answerId)
        {
            if (answerId < 0)
                throw new ArgumentException($"Wrong answer Id: {answerId}");

            return await answerRepository.Delete(answerId);
        }

        public async Task<Answer> GetById(int answerId)
        {
            if (answerId < 0)
                throw new ArgumentException($"Wrong answer Id: {answerId}");

            return await answerRepository.GetById(answerId);
        }

        public async Task<List<Answer>> GetByQuestion(int questionId)
        {
            if (questionId < 0)
                throw new ArgumentException($"Wrong question Id: {questionId}");

            return await answerRepository.GetByQuestion(questionId);
        }
    }
}
