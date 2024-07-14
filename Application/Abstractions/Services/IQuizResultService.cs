﻿using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface IQuizResultService
    {
        Task<QuizResult> SaveQuizResult(CreateQuizResultRequest request, string userId);
        Task<QuizResult> GetQuizResult(string userId, int quizId);
    }
}
