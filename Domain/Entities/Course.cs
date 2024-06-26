﻿using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class Course
    {
        public int Id { get ; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;
        public string? ImgPath { get; set; }
        public ICollection<Lesson>? Lessons { get; set; } 
        public ICollection<Quiz>? Quizes { get; set; }
    }
}
