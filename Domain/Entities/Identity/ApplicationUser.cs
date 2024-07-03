using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? PictureUrl { get; set; }
        public int Points { get; set; }

        public ICollection<Course>? Courses { get; set; } 
        public ICollection<QuizResult>? QuizResults { get; set; }
        public ICollection<UserAnswer>? UserAnswers { get; set; }
    }
}
 