using Domain.Entities;

namespace Application.interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesByAuthorAsync(int authorId);
    }
}
