using System.Diagnostics.CodeAnalysis;

namespace AuthorizationService.Core
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateOnly RegistrationDate { get; private set; }

        private User(
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

        public static User Create(
            Guid id, string firstName, string lastName, 
            DateOnly birthDate, string email, 
            string passwordHash, DateOnly registrationDate)
        {
            return new User(id, firstName, lastName, birthDate,
                email, passwordHash, registrationDate);
        }
    }
}
