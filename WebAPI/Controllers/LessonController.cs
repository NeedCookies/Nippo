﻿using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("lesson")]
    public class LessonController(ILessonsService lessonsService) : ControllerBase
    {
        [HttpGet("get-lessons-by-course")]
        public async Task<IActionResult> GetLessonsByCourse(int courseId)
        {
            var lessons = await lessonsService.GetByCourseId(courseId);
            return Ok(lessons);
        }

        [HttpPost("create-lesson")]
        public async Task<IActionResult> Create([FromBody] CreateLessonRequest request)
        {
            var lesson = await lessonsService.Create(request);

            return Ok(lesson);
        }
    }
}
