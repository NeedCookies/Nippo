﻿using Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ServiceRegisterExt
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ICoursesService, CoursesService>();
            services.AddScoped<ILessonsService, LessonsService>();
            services.AddScoped<IBlockService, BlockService>(); 
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAnswerService, UserAnswerService>();
            services.AddScoped<IQuizResultService, QuizResultService>();
            services.AddScoped<IUserProgressService, UserProgressService>();

            return services;
        }
    }
}
