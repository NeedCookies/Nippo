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
            services.AddTransient<ICoursesService, CoursesService>();
            return services;
        }
    }
}
