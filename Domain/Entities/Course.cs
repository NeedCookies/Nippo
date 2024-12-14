using Domain.Entities.Identity;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Course
    {
        public int Id { get ; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public Guid AuthorId { get; set; }
        public ApplicationUser Author { get; set; } = null!;
        public string? ImgPath { get; set; }
        public ICollection<Lesson>? Lessons { get; set; } 
        public ICollection<Quiz>? Quizes { get; set; }
        [JsonIgnore]
        public ICollection<UserCourses>? UserCourses { get; set; }
    }
}
