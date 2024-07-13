using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Application.Services
{
    public class LessonsService(
        ILessonRepository lessonRepository, 
        IUserCoursesRepository userCoursesRepository,
        ICourseRepository courseRepository,
        IUserService userService,
        IUserProgressRepository userProgressRepository) : ILessonsService
    {
        public async Task<Lesson> Create(CreateLessonRequest request)
        {
            string title = request.Title;
            int courseId = request.CourseId;
            DateTime date = DateTime.UtcNow;

            StringBuilder error = new StringBuilder("");

            if (title.Length == 0)
                error.AppendLine("Title shouldn't be empty");

            if (courseId < 0)
                error.AppendLine("Wrong course id");

            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            return await lessonRepository.Create(title, courseId, date);
        }

        public async Task<Lesson> Update(int lessonId, string title) =>
            await lessonRepository.Update(title, lessonId);

        public async Task<Lesson> Delete(int lessonId) =>
            await lessonRepository.Delete(lessonId);

        public async Task<Lesson> GetById(int lessonId, string userId)
        {
            StringBuilder error = new StringBuilder("");

            if (lessonId < 0)
                error.AppendLine("Wrong lesson id");

            if (error.Length > 0)
                throw new ArgumentException(error.ToString());

            var lesson = await lessonRepository.GetById(lessonId);

            if(await Validate(lesson.CourseId, userId))
            {
                var user = await userService.GetUserInfoById(userId);

                if (user.Role == "user")
                    await userProgressRepository.UpdateProgress(
                        new UserProgressRequest
                        (
                            userId, 
                            lesson.CourseId, 
                            lessonId, 
                            0
                         )
                    );

                return lesson;
            }
            else
                throw new Exception("Access denied");

        }

        public async Task<List<Lesson>> GetByCourseId(int courseId, string userId)
        {
            if(await Validate(courseId, userId))
                return await lessonRepository.GetLessonsByCourseAsync(courseId);
            else
                throw new Exception("Access denied");
        }

        private async Task<bool> Validate(int courseId, string userId)
        {
            bool isPurchased = await userCoursesRepository.IsCoursePurchased(userId, courseId);
            string courseAuthor = await courseRepository.GetAuthorById(courseId);

            bool result = false;

            if (isPurchased || courseAuthor == userId)
                result = true;

            return result;
        }
    }
}
