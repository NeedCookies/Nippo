﻿using Application.Abstractions.Repositories;
using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Extensions
{
    public static class RepositoryRegisterExt
    {
        public static IServiceCollection AddAppRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
