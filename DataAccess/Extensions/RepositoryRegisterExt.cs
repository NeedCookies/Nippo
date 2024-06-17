using Application.Abstractions.Repositories;
using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Extensions
{
    public static class RepositoryRegisterExt
    {
        public static IServiceCollection AddAppRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICourseRepository, CourseRepository>();
            return services;
        }
    }
}
