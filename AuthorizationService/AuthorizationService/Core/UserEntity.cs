using System.Diagnostics.CodeAnalysis;

namespace AuthorizationService.Core
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateOnly RegistrationDate { get; private set; }
        public ICollection<RoleEntity> Roles { get; set; } = [];

        private UserEntity(
            Guid id, string firstName, string lastName,
            DateOnly birthDate, string email, string passwordHash,
            DateOnly registrationDate)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Email = email;
            PasswordHash = passwordHash;
            RegistrationDate = registrationDate;
        }

        public static UserEntity Create(
            Guid id, string firstName, string lastName,
            DateOnly birthDate, string email,
            string passwordHash, DateOnly registrationDate)
        {
            return new UserEntity(id, firstName, lastName, birthDate,
                email, passwordHash, registrationDate);
        }
    }
}
