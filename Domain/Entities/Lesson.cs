using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string AuthorId { get; set; }
        public IdentityUser Author { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<Block> Blocks { get; set; }
    }
}
