using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<string>
    {
        public ICollection<Course>? Courses { get; set; } 
        public ICollection<QuizResult>? QuizResults { get; set; }
        public ICollection<UserAnswer>? UserAnswers { get; set; }
    }
}
