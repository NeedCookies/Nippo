using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Domain.Entities.Identity;

namespace Application.Services
{
    public class UserService(IPasswordHasher passwordHasher, IUserRepository userRepository, IJwtProvider jwtProvider) : IUserService
    {
        public async Task<string> Login(string userName, string password)
        {
            var user = await userRepository.GetByUserName(userName);

            var result = passwordHasher.Verify(password, user.PasswordHash);

            if(result == false)
            {
                throw new Exception("Failed to login");
            }

            var token = jwtProvider.Generate(user);

            return token;
        }

        public async Task<ApplicationUser> Register(string userName, string email, string password)
        {
            var hashedPassword = passwordHasher.Generate(password);
        
            var user = userRepository.Add(userName, email, hashedPassword);

            return await user;
        }
    }
}
