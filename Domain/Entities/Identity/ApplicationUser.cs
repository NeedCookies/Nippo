using System.Text.Json.Serialization;

namespace Domain.Entities.Identity
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// User's nickname
        /// </summary>
        public string? UserName { get; set; }
        public string? PictureUrl { get; set; }
        public int Points { get; set; }
        public AppRole Role { get; set; }
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
 