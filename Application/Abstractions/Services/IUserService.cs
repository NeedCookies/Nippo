using Domain.Entities;
﻿using Application.Contracts;
using Application.Contracts.Update;


namespace Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<List<Course>> GetUserCourses(string userId);
        Task<List<Course>> GetCreatedCourses(string userId);
        Task<List<UserProgress>> GetUserProgresses(string userId, int courseId);
        Task<PersonalInfoDto> GivePointsToUser(string userId, int points);
        Task<PersonalInfoDto> GetUserInfoById(string userId);
        Task<PersonalInfoDto> UpdateUserInfo(string userId, UpdateUserInfoRequest updateRequest);
        Task AssignRole(string userId, string roleId);
        Task UpgradeRoleToAuthor(string userId);
        Task DowngradeRoleToUser(string userId);
        Task<int> GetUsersByCourse(int courseId);
    }
}
