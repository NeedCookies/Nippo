using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Application.Contracts;
using Domain.Entities.Identity;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Application.Contracts.Update;

namespace Application.Services
{
    public class UserService(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IUserCoursesRepository userCoursesRepository,
        IUserRolesRepository userRolesRepository,
        ICourseRepository courseRepository,
        IStorageService storageService,
        IUserProgressRepository userProgressRepository,
        IDiscountService discountService,
        ILogger<UserService> logger) : IUserService
    {
        public async Task<PersonalInfoDto> GetUserInfoById(string userId)
        {
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"User Id has incorrect format: {userId}");

            var user = await userRepository.GetByUserId(guidUserId);

            if (user == null)
            {
                throw new ArgumentException("User with such Id was not found");
            }

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
                Role = user.Role.ToString(),
                UserName = user.UserName
            };
        }

        public async Task<PersonalInfoDto> GivePointsToUser(string userId, int points)
        {
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"User Id has incorrect format: {userId}");

            var user = await userRepository.GetByUserId(guidUserId);

            if (user == null)
            {
                throw new ArgumentException("User with such Id was not found");
            }

            user.Points += points;

            await unitOfWork.SaveChangesAsync();

            logger.LogWarning("Points were given to user. Points: {Points}. User Id: {UserId}", points, userId);

            return await GetUserInfoById(userId);
        }

        public async Task<List<Course>> GetUserCourses(string userId)
        {
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"User Id has incorrect format: {userId}");

            List<Course> userCourses = new List<Course>();
            var coursesId = await userCoursesRepository.GetUserCourses(guidUserId);

            // We need this code to fully get info about course by its id
            // That's why we don't use lambda here
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
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"User Id has incorrect format: {userId}");

            var courses = await courseRepository.GetCoursesByAuthorAsync(guidUserId);

            foreach (var course in courses)
            {
                course.ImgPath = course.ImgPath != null
                        ? await storageService.GetUrlAsync(course.ImgPath)
                        : null;
            }

            return courses;
        }

        public async Task<PersonalInfoDto> UpdateUserInfo(string userId, UpdateUserInfoRequest updateRequest)
        {
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"User Id has incorrect format: {userId}");

            var user = await userRepository.GetByUserId(guidUserId);

            if (user == null)
            {
                throw new ArgumentException("User with such Id was not found");
            }

            user.FirstName = updateRequest.FirstName ?? user.FirstName;
            user.LastName = updateRequest.LastName ?? user.LastName;
            user.UserName = updateRequest.UserName ?? user.UserName;
            user.PhoneNumber = updateRequest.PhoneNumber ?? user.PhoneNumber;
            user.BirthDate = updateRequest.BirthDate == null ?
                null 
                : DateOnly.ParseExact(updateRequest.BirthDate, "yyyy-MM-dd");

            if (updateRequest.UserPictureFile != null)
            {
                var pictureStream = updateRequest.UserPictureFile.OpenReadStream();
                var fileName = Guid.NewGuid().ToString();

                await storageService.PutAsync(fileName, pictureStream, updateRequest.UserPictureFile.ContentType);

                user.PictureUrl = fileName;
            }

            await unitOfWork.SaveChangesAsync();

            return await GetUserInfoById(userId);
        }

        public async Task<int> GetUsersByCourse(int courseId)
        {
            var userIds =  await userCoursesRepository.GetAcquiredUsers(courseId);

            return userIds.Count;
        }

        public async Task AssignRole(string userId, string roleId)
        {
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"User Id has incorrect format: {userId}");

            if (roleId == null || !Enum.TryParse<AppRole>(roleId, out var appRole))
                throw new ArgumentException($"There's no role with id: {roleId}");

            await userRolesRepository.AssignRole(guidUserId, appRole);

            logger.LogInformation("User was assigned a new role. User Id: {UserId}. New role Id: {RoleId}", userId, roleId);
        }

        public async Task UpgradeRoleToAuthor(string userId)
        {
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"User Id has incorrect format: {userId}");
            
            var user = await GetUserInfoById(userId);

            if (user.FirstName == null)
                throw new Exception("FirstName Field Is Empty!");
            if (user.LastName == null)
                throw new Exception("LastName Field Is Empty!");
            if (user.BirthDate == null)
                throw new Exception("BirthDate Field Is Empty!");
            if (user.PictureUrl == null)
                throw new Exception("PictureUrl Field Is Empty!");

            await userRolesRepository.AssignRole(guidUserId, AppRole.Author);

            logger.LogInformation("User role was changed to 'Author'. User Id: {UserId}.", userId);
        }

        public async Task DowngradeRoleToUser(string userId)
        {
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"User Id has incorrect format: {userId}");

            await userRolesRepository.SetDefaultRole(guidUserId);

            logger.LogInformation("User role was downgraded to 'User'. User Id: {UserId}.", userId);
        }

        public async Task<List<UserProgress>> GetUserProgresses(string userId, int courseId)
        {
            if (userId == null || !Guid.TryParse(userId, out var guidUserId))
                throw new ArgumentException($"User Id has incorrect format: {userId}");

            return await userProgressRepository.GetElementsByUserCourseId(guidUserId, courseId);
        }

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
    