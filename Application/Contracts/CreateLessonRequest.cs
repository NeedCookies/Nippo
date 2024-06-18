using Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public record CreateLessonRequest
        (
        int CourseId,
        string Title,
        string AuthorId
        );
}
