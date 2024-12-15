using Domain.Entities.Identity;

namespace Application.Abstractions.Repositories
{
    public interface IUserRolesRepository
    {
        Task AssignRole(Guid userId, AppRole role);
        Task SetDefaultRole(Guid userId);
        Task<AppRole> GetByUserId(Guid userId);
    }
}
