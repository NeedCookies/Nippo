using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Application.Services
{
    public class CoursesService(
        ICourseRepository courseRepository, 
        IUserCoursesRepository userCoursesRepository,
        IUserProgressRepository userProgressRepository,
        ILessonRepository lessonRepository,
        IQuizRepository quizRepository,
        IUserService userService) : ICoursesService
    {
        public async Task<Course> Create(CreateCourseRequest request, string authorId)
        {
            string title = request.Title;
            string descript = request.Description;
            decimal price = request.Price;
            string imgPath = request.ImgPath;

            StringBuilder error = new StringBuilder("");
            if (title.Length == 0)
            {
                error.Append("Title shouldn't be null");
            }
            if (descript.Length == 0)
            {
                error.Append("Description shouldn't be null");
            }
            if (price < 0)
            {
                error.Append("Prize should be equal 0 or more");
            }

            if (error.Length > 0)
            {
                throw new ArgumentException(error.ToString());
            }

            return await courseRepository.Create(title, descript, price, imgPath, authorId);
        }

        public async Task<Course> Update(UpdateCourseRequest request)
        {
            int id = request.Id;
            string title = request.Title;
            string descript = request.Description;
            decimal price = request.Price;
            string imgPath = request.ImgPath;

            return await courseRepository.Update(id, title, descript, price, imgPath);
        }

        public async Task<Course> Delete(int courseId) => 
            await courseRepository.Delete(courseId);

        public async Task<List<Course>> GetAllCourses() =>
            await courseRepository.GetAllCourses();

        public async Task<string> GetAuthorById(int id) =>
            await courseRepository.GetAuthorById(id);

        public async Task<List<Course>> GetCoursesToCheck() =>
            await courseRepository.GetCoursesByStatus(PublishStatus.Check);

        public async Task<Course> AcceptCourse(int courseId) =>
            await courseRepository.ChangeStatus(courseId, PublishStatus.Publish);

        public async Task<Course> CancelCourse(int courseId) =>
            await courseRepository.ChangeStatus(courseId, PublishStatus.Edit);

        public async Task<Course> SubmitForReview(int courseId, string userId)
        {
            var courseAuthor = await courseRepository.GetAuthorById(courseId);
            var userInfo = await userService.GetUserInfoById(userId);

            if (courseAuthor == userId && userInfo.Role == "author")
                return await courseRepository.ChangeStatus(courseId, PublishStatus.Check);
            else
                throw new Exception("Access denied");
        }

        public async Task<Course> GetById(int id)
        {
            var course = await courseRepository.GetById(id);

            if (course == null)
            {
                throw new Exception("Course with such id was not found");
            }

            return course;
        }

        public async Task<UserCourses> PurchaseCourse(int courseId, string userId)
        {
            var courseLessons = await lessonRepository.GetLessonsByCourseAsync(courseId);
            var courseQuizzes = await quizRepository.GetQuizzesByCourseAsync(courseId);
            List<UserProgressRequest> request = new List<UserProgressRequest>();

            int courseSize = courseLessons.Count + courseQuizzes.Count;

            for(int i = 0; i < courseLessons.Count; i++)
            {
                UserProgressRequest newRequest = new UserProgressRequest
                (
                    userId,
                    courseId,
                    courseLessons[i].Id,
                    0
                );

                request.Add(newRequest);
            }

            for (int i = 0; i < courseQuizzes.Count; i++)
            {
                UserProgressRequest newRequest = new UserProgressRequest
                (
                    userId,
                    courseId,
                    courseQuizzes[i].Id,
                    1
                );
             
                request.Add(newRequest);
            }

            await userProgressRepository.AddedAll(request);

            return await userCoursesRepository.Add(courseId, userId);
        }
    }
}
