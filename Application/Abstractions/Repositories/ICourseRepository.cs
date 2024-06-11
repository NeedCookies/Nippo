using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesByAuthorAsync(int authorId);
    }
}
