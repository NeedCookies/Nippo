using Application.Abstractions.Repositories;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        public async Task<ApplicationUser> Add(string userName, string email, string password)
        {
            var id = Guid.NewGuid().ToString();

            var user = new ApplicationUser()
            {
                Id = id,
                UserName = userName,
                Email = email,
                PasswordHash = password
            };

            await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser> GetByUserName(string userName)
        {
            var userEntity = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == userName)?? throw new Exception();

            return userEntity;
        }
    }
}
