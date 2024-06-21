using Application.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface ILessonsService
    {
        Task<List<Lesson>> GetByCourseId(int courseId);
        Task<Lesson> GetById(int courseId, int lessonId);
        Task<Lesson> Create(CreateLessonRequest request);
    }
}
