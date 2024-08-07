﻿using Application.Abstractions.Services;
using Application.Contracts;
using Application.Contracts.Update;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionController(IQuestionService questionService) : ControllerBase
    {
        [HttpGet("get-by-quiz")]
        public async Task<IActionResult> GetByQuiz(int quizId)
        {
            var questions = await questionService.GetByQuizId(quizId);
            return Ok(questions);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById(int questionId)
        {
            var question = await questionService.GetById(questionId);
            return Ok(question);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateQuestionRequest request)
        {
            var question = await questionService.Create(request);
            return Ok(question);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int questionId)
        {
            var question = await questionService.Delete(questionId);
            return Ok(question);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateQuestionRequest request)
        {
            var question = await questionService.Update(request);
            return Ok(question);
        }
    }
}
