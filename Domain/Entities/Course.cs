using Microsoft.AspNet.Identity.EntityFramework;

namespace Domain.Entities
{
    public class Course
    {
        public int Id { get ; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public string ImgPath { get; set; }
    }
}
