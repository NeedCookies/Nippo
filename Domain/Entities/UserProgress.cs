using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class UserProgress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int? LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        public int? QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public bool IsCheck { get; set; }
    }
}
