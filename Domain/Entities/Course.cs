using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Course
    {
        public int Id { get ; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public string ImgPath { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Quiz> Quizes { get; set; }
    }
}
