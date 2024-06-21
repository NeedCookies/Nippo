using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface ICoursesService
    {
        public Task<List<Course>> GetAllCourses();
        public Task<Course> Create(CreateCourseRequest request);
        public Task<Course> GetById(int id);
    }
}
