﻿using Domain.Entities.Identity;

namespace Domain.Entities
{
    public class BasketCourses
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
