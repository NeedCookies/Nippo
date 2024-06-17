using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public record GetCourseResponse
        (
        int Id,
        string Title,
        string Description,
        decimal Price,
        string ImgPath
        );
}
