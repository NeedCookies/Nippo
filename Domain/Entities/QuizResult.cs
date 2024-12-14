using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class QuizResult
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public int Score { get; set; }
        public int Attempt { get; set; }
    }
}
