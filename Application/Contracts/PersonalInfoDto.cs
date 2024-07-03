﻿namespace Application.Contracts
{
    public class PersonalInfoDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int Points { get; set; }
        public string Role { get; set; } = null!;
    }
}