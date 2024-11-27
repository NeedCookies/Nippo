using Infrastructure.Entities;

namespace Application.Abstractions.Services
{
    public interface IAuthServiceHttp
    {
        Task<HashSet<Permission>> GetUserPermissionsAsync(Guid userId);
    }
}
