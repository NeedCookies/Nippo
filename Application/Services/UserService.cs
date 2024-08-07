﻿using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Application.Contracts;
using Domain.Entities.Identity;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserService(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IUserCoursesRepository userCoursesRepository,
        IUserRolesRepository userRolesRepository,
        ICourseRepository courseRepository,
        UserManager<ApplicationUser> userManager,
        RoleManager<AppRole> roleManager,
        IStorageService storageService,
        IUserProgressRepository userProgressRepository,
        IDiscountService discountService,
        ILogger<UserService> logger) : IUserService
    {
        public async Task<PersonalInfoDto> GetUserInfoById(string userId)
        {
            var user = await userRepository.GetByUserId(userId);

            if (user == null)
            {
                throw new ArgumentException("User with such Id was not found");
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var pictureUrl = user.PictureUrl != null
                ? await storageService.GetUrlAsync(user.PictureUrl)
                : null;

            return new PersonalInfoDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate.HasValue
                    ? user.BirthDate.Value.ToString("yyyy-MM-dd")
                    : null,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PictureUrl = pictureUrl,
                Points = user.Points,
                Role = userRoles.First(),
                UserName = user.UserName
            };
        }

        public async Task<PersonalInfoDto> GivePointsToUser(string userId, int points)
        {
            var user = await userRepository.GetByUserId(userId);

            if (user == null)
            {
                throw new ArgumentException("User with such Id was not found");
            }

            user.Points += points;

            await unitOfWork.SaveChangesAsync();

            logger.LogWarning("Points were given to user. Points: {Points}. User Id: {UserId}", points, userId);

            return await GetUserInfoById(userId);
        }

        public async Task<string> Login(string userName, string password)
        {
            var user = await userRepository.GetByUserName(userName);

            var result = passwordHasher.Verify(password, user.PasswordHash!);

            if (result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = await jwtProvider.GenerateAsync(user);

            logger.LogInformation("User were logged in. User Id: {UserId}", user.Id);
            return token;
        }

        public async Task<ApplicationUser> Register(string userName, string email, string password)
        {
            if (IsValidEmail(email) == false)
            {
                throw new ArgumentException("Invalid email address.");
            }

            if (IsValidPassword(password) == false)
            {
                throw new ArgumentException("Password does not meet the requirements.");
            }

            var hashedPassword = passwordHasher.Generate(password);

            var user = await userRepository.Add(userName, email, hashedPassword);

            var registeredUser = await userRepository.GetByUserName(userName);

            var defaultRole = await userRepository.GetDefaultUserRole();

            await userRolesRepository.AssignRole(registeredUser.Id, defaultRole.Id);

            logger.LogInformation("A new user has registered. UserId: {UserId}, Role: {Role}", user.Id, defaultRole.Name);

            return user;
        }

        public async Task<List<Course>> GetUserCourses(string userId)
        {
            List<Course> userCourses = new List<Course>();
            var coursesId = await userCoursesRepository.GetUserCourses(userId);

            foreach (var courseId in coursesId)
            {
                var course = await courseRepository.GetById(courseId);

                if (course != null)
                {
                    course.ImgPath = course.ImgPath != null 
                        ? await storageService.GetUrlAsync(course.ImgPath) 
                        : null;
                    userCourses.Add(course);
                }
            }

            return userCourses;
        }

        public async Task<List<Course>> GetCreatedCourses(string userId)
        {
            var courses = await courseRepository.GetCoursesByAuthorAsync(userId);

            foreach (var course in courses)
            {
                course.ImgPath = course.ImgPath != null
                        ? await storageService.GetUrlAsync(course.ImgPath)
                        : null;
            }

            return courses;
        }

        public async Task<PersonalInfoDto> UpdateUserInfo(string userId, UserInfoUpdateRequest updateRequest)
        {
            var user = await userRepository.GetByUserId(userId);

            if (user == null)
            {
                throw new ArgumentException("User with such Id was not found");
            }

            user.FirstName = updateRequest.FirstName;
            user.LastName = updateRequest.LastName;
            user.PhoneNumber = updateRequest.PhoneNumber;
            user.BirthDate = updateRequest.BirthDate == null
                ? null
                : DateOnly.ParseExact(updateRequest.BirthDate, "yyyy-MM-dd");

            if (updateRequest.UserPictureFile != null)
            {
                var pictureStream = updateRequest.UserPictureFile.OpenReadStream();
                var fileName = Guid.NewGuid().ToString();

                await storageService.PutAsync(fileName, pictureStream, updateRequest.UserPictureFile.ContentType);

                user.PictureUrl = fileName;
            }

            await unitOfWork.SaveChangesAsync();

            return await GetUserInfoById(user.Id);
        }

        public async Task<int> GetUsersByCourse(int courseId)
        {
            var userIds =  await userCoursesRepository.GetAcquiredUsers(courseId);

            return userIds.Count;
        }

        public async Task<List<GetUsersAndRolesRequest>> GetUsersAndRoles()
        {
            var users = await userRepository.GetAllUsers();
            var roles = await roleManager.Roles.ToListAsync();
            var userRoles = await userRolesRepository.GetUsersAndRoles();

            var userDict = users.ToDictionary(u => u.Id, u => u.UserName);
            var roleDict = roles.ToDictionary(r => r.Id, r => r.Name);

            List<GetUsersAndRolesRequest> usersAndRoles = new List<GetUsersAndRolesRequest>();

            foreach (var userRole in userRoles)
            {
                if (userDict.TryGetValue(userRole.UserId, out var userName) &&
                    roleDict.TryGetValue(userRole.RoleId, out var roleName))
                {
                    usersAndRoles.Add(new GetUsersAndRolesRequest(
                        userRole.UserId, 
                        userName, 
                        roleName));
                }
            }

            return usersAndRoles;
        }

        public async Task<List<ApplicationUser>> GenerateUsers()
        {
            const int numberOfNewUsers = 5;
            List<ApplicationUser> users = new List<ApplicationUser>();

            for (int i = 0; i < numberOfNewUsers; i++)
            {
                string userName = $"user{i}_{Guid.NewGuid().ToString().Substring(0, 4)}";
                string email = $"{userName}@example.com";
                string defaultPassword = "Password123";

                await Register(userName, email, defaultPassword);

                users.Add(await userRepository.GetByUserName(userName));
            }

            return users;
        }

        public async Task AssignRole(string userId, string roleId)
        {
            if(await userRolesRepository.GetByUserId(userId) != null)
            {
                await userRolesRepository.RemoveRole(userId);
            }

            await userRolesRepository.AssignRole(userId, roleId);

            logger.LogInformation("User was assigned a new role. User Id: {UserId}. New role Id: {RoleId}", userId, roleId);
        }

        public async Task UpgradeRoleToAuthor(string userId)
        {
            var user = await GetUserInfoById(userId);
            string authorRoleId = "2";

            if (user.FirstName == null)
                throw new Exception("FirstName Field Is Empty!");
            if (user.LastName == null)
                throw new Exception("LastName Field Is Empty!");
            if (user.BirthDate == null)
                throw new Exception("BirthDate Field Is Empty!");
            if (user.PictureUrl == null)
                throw new Exception("PictureUrl Field Is Empty!");

            await userRolesRepository.RemoveRole(userId);
            await userRolesRepository.AssignRole(userId, authorRoleId);

            logger.LogInformation("User role was changed to 'Author'. User Id: {UserId}.", userId);
        }

        public async Task DowngradeRoleToUser(string userId)
        {
            string userRoleId = "1";

            await userRolesRepository.RemoveRole(userId);
            await userRolesRepository.AssignRole(userId, userRoleId);

            logger.LogInformation("User role was downgraded to 'User'. User Id: {UserId}.", userId);
        }

        public async Task<List<UserProgress>> GetUserProgresses(string userId, int courseId) =>
            await userProgressRepository.GetElementsByUserCourseId(userId, courseId);

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private bool IsValidPassword(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[a-zа-я])(?=.*[A-ZА-Я])(?=.*\d)[A-Za-zА-Яа-я\d]{8,}$");

            return passwordRegex.IsMatch(password);
        }
    }
}
    