using Application.Abstractions.Services;

namespace Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password) => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string password, string hasherPassword) => BCrypt.Net.BCrypt.EnhancedVerify(password, hasherPassword);
    }
}
