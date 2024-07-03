using Application.Contracts;
using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> Register(string userName, string email, string password);
        Task<string> Login(string userName, string password);
        Task<PersonalInfoDto> GetUserInfoById(string userId);
    }
}
