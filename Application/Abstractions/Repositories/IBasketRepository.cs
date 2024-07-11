
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IBasketRepository
    {
        Task<List<BasketCourses>> GetBasketCourses(string userId);
        Task<BasketCourses> AddtoBasket(int courseId, string UserId);
        Task<BasketCourses> DeleteFromBasket(int courseId, string UserId);
    }
}
