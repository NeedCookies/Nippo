using AuthorizationService.Application.Abstractions;
using AuthorizationService.Application.Contracts;
using AuthorizationService.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace AuthorizationService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(IUserRepository userRepository, 
            IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new NullReferenceException(nameof(user) + " isn't exist");
            }

            var result = _passwordHasher.Verify(password, user.PasswordHash);
            if (result == false)
            {
                throw new AuthenticationException("Wrong password");
            }

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }

        public async Task RegisterUserAsync(
            string firstName, string lastName, DateOnly birthDate, string email, string password)
        {
            var hashedPassword = _passwordHasher.Generate(password);

            var user = User.Create(
                Guid.NewGuid(), firstName, lastName, birthDate, email,
                hashedPassword, DateOnly.FromDateTime(DateTime.Now));

            await _userRepository.AddAsync(user);
        }
    }
}
