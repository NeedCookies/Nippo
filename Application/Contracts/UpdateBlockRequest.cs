using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public record UpdateBlockRequest
    (
    int Id,
    int Type,
    string Content,
    int Order
    );
}
