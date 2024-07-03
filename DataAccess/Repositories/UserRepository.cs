using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                PasswordHash = password,
                Money = 1500
            };

            await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task AssignRole(ApplicationUser user, string roleId)
        {
            var userRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = roleId
            };

            appDbContext.UserRoles.Add(userRole);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var userEntity = await appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id)
                ?? throw new Exception();

            return userEntity;
        }

        public async Task<ApplicationUser> GetUserByUserName(string userName)
        {
            var userEntity = await appDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName)
                ?? throw new Exception();

            return userEntity;
        }

        public async Task<AppRole> GetDefaultUserRole()
        {
            const string userRoleName = "admin";
            var userRole = await appDbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == userRoleName)
                ?? throw new Exception();

            return userRole;
        }

        public async Task<List<Course>> GetUserCourses(string userId)
        {
            var user = await appDbContext.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user.Courses.ToList();
        }
    }
}
