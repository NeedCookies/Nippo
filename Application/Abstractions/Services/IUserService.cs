using Domain.Entities;
﻿using Application.Contracts;


namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        /*
        Task<ApplicationUser> Register(string userName, string email, string password);
        Task<PersonalInfoDto> UpdateUserInfo(string userId, UserInfoUpdateRequest updateRequest, Stream pictureStream);
        Task<string> Login(string userName, string password);
        */
        Task<List<Course>> GetUserCourses(string userId);
        Task<PersonalInfoDto> GivePointsToUser(string userId, int points);
        Task<PersonalInfoDto> GetUserInfoById(string userId);
    }
}
