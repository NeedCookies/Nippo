using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using System.Text;

namespace Application.Services
{
    public class LessonsService(ILessonRepository lessonRepository) : ILessonsService
    {
        public async Task<Lesson> Create(CreateLessonRequest request)
        {
            string title = request.Title;
            int courseId = request.CourseId;
            string authorId = request.AuthorId;
            DateTime date = DateTime.UtcNow;

            StringBuilder error = new StringBuilder("");
            if (title.Length == 0)
            {
                error.AppendLine("Title shouldn't be empty");
            }
            if (courseId < 0)
            {
                error.AppendLine("Wrong course id");
            }
            if (error.Length > 0)
            {
                throw new ArgumentException(error.ToString());
            }

            return await lessonRepository.Create(title, courseId, authorId, date);
        }

        public async Task<Lesson> GetById(int courseId, int lessonId)
        {
            StringBuilder error = new StringBuilder("");
            if (courseId < 0) { error.AppendLine("Wrong course id"); }
            if (lessonId < 0) { error.AppendLine("Wrong lesson id"); }
            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            return await lessonRepository.GetById(courseId, lessonId);
        }

        public async Task<List<Lesson>> GetByCourseId(int courseId)
        {
            if (courseId < 0) { throw new ArgumentException("Wrong course id"); }

            return await lessonRepository.GetLessonsByCourseAsync(courseId);
        }
    }
}
