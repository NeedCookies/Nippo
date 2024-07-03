using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<string>
    {
        [JsonIgnore]
        public ICollection<Course>? Courses { get; set; } 
        public ICollection<QuizResult>? QuizResults { get; set; }
        public ICollection<UserAnswer>? UserAnswers { get; set; }
        public decimal Money { get; set; }
    }
}
 