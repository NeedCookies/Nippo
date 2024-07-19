using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesByAuthorAsync(string authorId);
        Task<List<Course>> GetAllCourses();
        Task<Course> Create(string title, string desc, decimal price, string imgPath, string authorId);
        Task<Course> Update(int id, string title, string desc, decimal price, string imgPath);
        Task<Course> Delete(int id);
        Task<Course?> GetById(int id);
        Task<string> GetAuthorById(int id);
        Task<List<Course>> GetCoursesByStatus(PublishStatus status);
        Task<Course> ChangeStatus(int courseId, PublishStatus status);
    }
}
