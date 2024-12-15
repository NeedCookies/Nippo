using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class BasketCourses
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
