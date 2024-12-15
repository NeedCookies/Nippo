using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class DiscountService(IDistributedCache redisCache) : IDiscountService
    {
        public async Task AddPromocode(PromocodeDto promocode)
        {
            var promocodeEntity = new PromocodeEntity()
            {
                PromocodeType = promocode.PromocodeType,
                Discount = promocode.Discount
            };

            var jsonPromocodeEntity = JsonSerializer.SerializeToUtf8Bytes(promocodeEntity);

            await redisCache.SetAsync(promocode.Code, jsonPromocodeEntity, 
                new DistributedCacheEntryOptions() 
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(promocode.ExpirationInHours)  
                });
        }

        public async Task<int> ApplyPromocode(Course course, string promocode)
        {
            var newPrice = (int)course.Price;

            var promocodeEntityBytes = await redisCache.GetAsync(promocode);

            if (promocodeEntityBytes == null)
            {
                return newPrice;
            }

            var promocodeEntity = JsonSerializer.Deserialize<PromocodeEntity>(promocodeEntityBytes)!;

            if (promocodeEntity.PromocodeType == PromocodeType.Percent)
            {
                newPrice = int.Parse(
                    Math.Round(newPrice * ((100 - promocodeEntity.Discount) / 100.0)).ToString());
            }
            else
            {
                newPrice -= promocodeEntity.Discount;
            }

            //На случай, если вдруг скидка будет больше, чем сама цена курса
            return Math.Max(newPrice, 0);
        }

        private class PromocodeEntity
        {
            public PromocodeType PromocodeType { get; set; }
            public int Discount { get; set; }
        }
    }
}
