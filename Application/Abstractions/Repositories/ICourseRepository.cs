using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesByAuthorAsync(int authorId);
        Task<List<Course>> GetAllCourses();
        Task<Course> Create(string title, string desc, decimal price, string imgPath);
        Task<Course> GetById(int id);
    }
}
