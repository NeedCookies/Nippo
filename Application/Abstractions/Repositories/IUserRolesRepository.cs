using Application.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstractions.Repositories
{
    public interface IUserRolesRepository
    {
        Task AssignRole(string userId, string roleId);
        Task RemoveRole(string userId);
        Task<IdentityUserRole<string>> GetByUserId(string userId);

        Task<List<IdentityUserRole<string>>> GetUsersAndRoles();
    }
}
