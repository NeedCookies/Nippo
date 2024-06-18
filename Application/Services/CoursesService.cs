using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;

namespace Application.Services
{
    public class CoursesService(ICourseRepository courseRepository) : ICoursesService
    {

    }
}
