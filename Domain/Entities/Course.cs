﻿using Domain.Entities.Identity;
using System.Text.Json.Serialization;

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
        public int Status { get; set; }
        [JsonIgnore]
        public ICollection<Lesson>? Lessons { get; set; }
        [JsonIgnore]
        public ICollection<Quiz>? Quizes { get; set; }
        [JsonIgnore]
        public ICollection<UserCourses>? UserCourses { get; set; }
        [JsonIgnore]
        public ICollection<UserProgress>? UserProgresses { get; set; }
        [JsonIgnore]
        public ICollection<BasketCourses>? BasketCourses { get; set; }
    }

    public enum PublishStatus
    {
        Edit,
        Check,
        Publish
    }
}
