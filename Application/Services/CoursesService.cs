using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;

namespace Application.Services
{
    public class CoursesService(ICourseRepository courseRepository) : ICoursesService
    {
        public Task<List<Course>> GetAllCourses()
        {
            var allCourses = courseRepository.GetAllCourses();

            if (allCourses == null)
            {
                throw new Exception();
            }

            return allCourses;
        }
    }
}
