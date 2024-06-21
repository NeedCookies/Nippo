using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public record CreateBlockRequest
        (
        int LessonId,
        int Type,
        string Content,
        int Order
        );
}
