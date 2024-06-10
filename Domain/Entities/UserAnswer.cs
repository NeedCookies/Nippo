using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    internal class UserAnswer : Answer
    {
        public string StringUserId { get; set; }
        public IdentityUser User { get; set; }
        public int Attempt { get; set; }
    }
}
