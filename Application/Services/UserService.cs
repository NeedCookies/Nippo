using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities.Identity;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class UserService(
        IPasswordHasher passwordHasher, 
        IUserRepository userRepository, 
        IJwtProvider jwtProvider) : IUserService
    {
        public async Task<string> Login(string userName, string password)
        {
            var user = await userRepository.GetByUserName(userName);

            var result = passwordHasher.Verify(password, user.PasswordHash);

            if(result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = await jwtProvider.Generate(user);

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
        
            var user = userRepository.Add(userName, email, hashedPassword);

            var registeredUser = await userRepository.GetByUserName(userName);

            var defaultRole = await userRepository.GetDefaultUserRole();

            await userRepository.AssignRole(registeredUser, defaultRole.Id);

            return await user;
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
    