using Infrastructure.Entities;

namespace Application.Abstractions.Services
{
    public interface IAuthServiceHttp
    {
        Task<string> GetUserPermissionsAsync(Guid userId);
    }
}
