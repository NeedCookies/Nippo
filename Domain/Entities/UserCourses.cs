
using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class UserCourses
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
