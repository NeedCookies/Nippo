using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string Text { get; set; } = null!;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public int Attempt { get; set; }
    }
}
