using Domain.Entities;
using Domain.Entities.Identity;

namespace Application.Abstractions.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesByAuthorAsync(int authorId);
        Task<List<Course>> GetAllCourses();
        Task<Course> Create(string title, string desc, decimal price, string imgPath, string authorId);
        Task<Course> Update(int id, string title, string desc, decimal price, string imgPath);
        Task<Course> Delete(int id);
        Task<Course?> GetById(int id);
        /// <summary>
        /// Represents course purchasing process
        /// Add course to user.Courses if enough mony
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApplicationUser> PurchaseCourse(int courseId, string userId);
    }
}
