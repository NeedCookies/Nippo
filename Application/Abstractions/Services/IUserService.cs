using Application.Contracts;
using Domain.Entities.Identity;

namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> Register(string userName, string email, string password);
        Task<string> Login(string userName, string password);
        Task<PersonalInfoDto> GivePointsToUser(string userId, int points);
        Task<PersonalInfoDto> GetUserInfoById(string userId);
        Task<PersonalInfoDto> UpdateUserInfo(string userId, UserInfoUpdateRequest updateRequest, Stream pictureStream);
    }
}
