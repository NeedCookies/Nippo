using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities;
using Application.Contracts;
using Domain.Entities.Identity;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService(
        IPasswordHasher passwordHasher, 
        IUserRepository userRepository, 
        IJwtProvider jwtProvider,
        IUserCoursesRepository userCoursesRepository,
        ICourseRepository courseRepository,
        UserManager<ApplicationUser> userManager) : IUserService
    {
        public async Task<PersonalInfoDto> GetUserInfoById(string userId)
        {
            var user = await userRepository.GetByUserId(userId);

            if (user == null)
            {
                throw new ArgumentException("User with such Id was not found");
            }

            var userRoles = await userManager.GetRolesAsync(user);

            return new PersonalInfoDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Email = user.Email,
                PictureUrl = user.PictureUrl,
                Points = user.Points,
                Role = userRoles.First(),
                UserName = user.UserName
            };
        }

        public async Task<string> Login(string userName, string password)
        {
            var user = await userRepository.GetByUserName(userName);

            var result = passwordHasher.Verify(password, user.PasswordHash!);

            if(result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = await jwtProvider.GenerateAsync(user);

            return token;
        }

        public async Task<ApplicationUser> Register(string userName, string email, string password)
        {
            if(IsValidEmail(email) == false) 
            {
                throw new ArgumentException("Invalid email address.");
            }

            if(IsValidPassword(password) == false)
            {
                throw new ArgumentException("Password does not meet the requirements.");
            }

            var hashedPassword = passwordHasher.Generate(password);
        
            var user = await userRepository.Add(userName, email, hashedPassword);

            var registeredUser = await userRepository.GetByUserName(userName);

            var defaultRole = await userRepository.GetDefaultUserRole();

            await userRepository.AssignRole(registeredUser, defaultRole.Id);

            return user;
        }

        public async Task<List<Course>> GetUserCourses(string userId)
        {
            List<Course> userCourses = new List<Course>();
            var coursesId = await userCoursesRepository.GetUserCourses(userId);

            foreach(var courseId in coursesId) 
            {
                var course = await courseRepository.GetById(courseId);

                if(course != null)
                {
                    userCourses.Add(course);
                }
            }

            return userCourses;
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private bool IsValidPassword(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[a-zА-Я])(?=.*[A-ZА-Я])(?=.*\d)[A-Za-zА-Яа-я\d]{8,}$");

            return passwordRegex.IsMatch(password);
        }
    }
}
    