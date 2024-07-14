using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? PictureUrl { get; set; }
        public int Points { get; set; }
        [JsonIgnore]
        public ICollection<Course>? Courses { get; set; }
        [JsonIgnore]
        public ICollection<UserCourses>? UserCourses { get; set; }
        [JsonIgnore]
        public ICollection<QuizResult>? QuizResults { get; set; }
        [JsonIgnore]
        public ICollection<UserAnswer>? UserAnswers { get; set; }
        [JsonIgnore]
        public ICollection<UserProgress>? UserProgresses { get; set; }
        public ICollection<BasketCourses>? BasketCourses { get; set; }
    }
}
 