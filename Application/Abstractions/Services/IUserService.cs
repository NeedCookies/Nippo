using Domain.Entities;
using Domain.Entities.Identity;
﻿using Application.Contracts;


namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> Register(string userName, string email, string password);
        Task<string> Login(string userName, string password);
        Task<List<Course>> GetUserCourses(string userId);
        Task<PersonalInfoDto> GivePointsToUser(string userId, int points);
        Task<PersonalInfoDto> GetUserInfoById(string userId);
        Task<PersonalInfoDto> UpdateUserInfo(string userId, UserInfoUpdateRequest updateRequest, Stream pictureStream);
    }
}
