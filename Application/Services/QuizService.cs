using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using System.Text;

namespace Application.Services
{
    public class QuizService(IQuizRepository quizRepository) : IQuizService
    {
        public async Task<Quiz> Create(CreateQuizRequest request)
        {
            int courseId = request.courseId;
            string title = request.Title;
            
            StringBuilder error = new StringBuilder();
            if (courseId < 0) error.AppendLine("Wrong course Id");
            if (title == null || title.Length == 0)
                error.AppendLine("Title shouldn't be empty");

            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            return await quizRepository.Create(courseId, title);
        }

        public async Task<List<Quiz>> GetByCourseId(int courseId)
        {
            if (courseId < 0)
                throw new ArgumentException("Wrong course Id");

            return await quizRepository.GetQuizzesByCourseAsync(courseId);
        }

        public async Task<Quiz> GetById(int quizId)
        {
            if (quizId < 0)
                throw new ArgumentException("Wrong quiz Id");

            return await quizRepository.GetQuizByIdAsync(quizId);
        }

        public async Task<Quiz> Delete(int quizId)
        {
            if (quizId < 0)
                throw new ArgumentException("Wrong quiz Id");

            return await quizRepository.Delete(quizId);
        }
    }
}
