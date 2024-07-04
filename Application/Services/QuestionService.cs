using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using System.Text;

namespace Application.Services
{
    public class QuestionService(IQuestionRepository questionRepository) : IQuestionService
    {
        public async Task<Question> Create(CreateQuestionRequest request)
        {
            int quizId = request.QuizId;
            string text = request.Text;
            var type = request.Type;
            
            StringBuilder error = new StringBuilder();
            if (quizId < 0) error.AppendLine("wrong quiz Id");
            if (text == null || text.Length == 0)
                error.AppendLine("Question text shouldn't be empty");
            if (!Enum.IsDefined(typeof(QuestionType), type))
                error.AppendLine("Bad question type");

            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            int order = (await questionRepository.GetByQuiz(quizId)).Count() + 1;

            return await questionRepository.Create(order, quizId, text, type);
        }

        public async Task<Question> Delete(int questionId)
        {
            if (questionId < 0)
                throw new ArgumentException("Wrong question Id");

            return await questionRepository.Delete(questionId);
        }

        public async Task<Question> GetById(int questionId)
        {
            if (questionId < 0)
                throw new ArgumentException("Wrong question Id");

            return await questionRepository.GetById(questionId);
        }

        public async Task<List<Question>> GetByQuizId(int quizId)
        {
            if (quizId < 0)
                throw new ArgumentException("Wrong question Id");

            return await questionRepository.GetByQuiz(quizId);
        }

        public async Task<Question> Update(int questionId, string text)
        {
            StringBuilder error = new StringBuilder();
            if (questionId < 0)
                error.AppendLine("Wrong question Id");
            if (text == null || text.Length == 0)
                error.AppendLine("Question text shouldn't be empty");

            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            return await questionRepository.Update(questionId, text);
        }
    }
}
