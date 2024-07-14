using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface IDiscountService
    {
        Task AddPromocode(PromocodeDto promocode);
        Task<int> ApplyPromocode(Course course, string promocode);
    }
}
