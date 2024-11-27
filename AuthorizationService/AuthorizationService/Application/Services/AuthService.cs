using AuthorizationService.Application.Abstractions;
using AuthorizationService.Core;
using System.Security.Authentication;

namespace AuthorizationService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPermissionService _permissionService;

        public AuthService(
            IUserRepository userRepository, IPermissionService permissionService,
            IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _permissionService = permissionService;
        }

        public async Task<HashSet<Permission>> GetUserPermissionsAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var userGuidId))
            {
                throw new BadHttpRequestException("Cannot convert user id to guid");
            }
            var permissions = await _permissionService.GetPermissionsAsync(userGuidId);
            return permissions;
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
            if (token == null)
            {
                throw new Exception("Can't generate token");
            }

            return token;
        }

        public async Task RegisterUserAsync(
            string firstName, string lastName, DateOnly birthDate, string email, string password)
        {
            var checkUser = await _userRepository.GetByEmailAsync(email);
            if (checkUser != null)
            {
                throw new AuthenticationException("Account already exist");
            }

            var hashedPassword = _passwordHasher.Generate(password);

            var user = UserEntity.Create(
                Guid.NewGuid(), firstName, lastName, birthDate, email,
                hashedPassword, DateOnly.FromDateTime(DateTime.Now));

            await _userRepository.AddAsync(user);
        }
    }
}
