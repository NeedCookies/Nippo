using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface ICoursesService
    {
        public Task<List<Course>> GetAllCourses();
        public Task<Course> Create(CreateCourseRequest request, string authorId);
        public Task<Course> Update(UpdateCourseRequest request);
        public Task<Course> Delete(int id);
        public Task<Course> GetById(int id);
        /// <summary>
        /// Purchase course for user
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<UserCourses> PurchaseCourse(int courseId, string userId);
    }
}
