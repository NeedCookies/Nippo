using Microsoft.AspNet.Identity.EntityFramework;

namespace Domain.Entities
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int Attempt { get; set; }
    }
}
