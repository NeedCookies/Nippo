using Domain.Entities;
using Domain.Entities.Identity;

namespace Application.Abstractions.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesByAuthorAsync(int authorId);
        Task<List<Course>> GetAllCourses();
        Task<Course> Create(string title, string desc, decimal price, string imgPath, string authorId);
        Task<Course?> GetById(int id);
        Task<ApplicationUser> PurchaseCourse(int courseId, string userId);
    }
}
