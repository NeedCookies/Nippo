using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IBasketRepository
    {
        Task<List<BasketCourses>> GetBasketCourses(Guid userId);
        Task<BasketCourses> AddtoBasket(int courseId, Guid userId);
        Task<BasketCourses> DeleteFromBasket(int courseId, Guid userId);
        Task<BasketCourses?> GetBasketCourse(int courseId, Guid userId);
    }
}
