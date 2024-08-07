﻿using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Quiz
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int Order { get; set; } 
        public ICollection<Question> Questions { get; set; } = null!;
        public ICollection<QuizResult>? QuizResults { get; set; }
        [JsonIgnore]
        public ICollection<UserProgress>? UserProgresses { get; set; }
    }
}
