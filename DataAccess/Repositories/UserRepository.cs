using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    /*
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<ApplicationUser> Add(string userName, string email, string password)
        {
            var id = Guid.NewGuid().ToString();

            var user = new ApplicationUser()
            {
                Id = id,
                UserName = userName,
                Email = email,
                PasswordHash = password,
                Points = 1500
            };

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task AssignRole(ApplicationUser user, string roleId)
        {
            var userRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = roleId
            };

            _appDbContext.UserRoles.Add(userRole);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser?> GetByUserId(string userId)
        {
            var user = await appDbContext.Users.FindAsync(userId);

            return user;
        }

        public async Task<ApplicationUser> GetByUserName(string userName)
        {
            var userEntity = await _appDbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName)
                ?? throw new Exception();

            return userEntity;
        }

        public async Task<AppRole> GetDefaultUserRole()
        {
            const string userRoleName = "admin";
            var userRole = await _appDbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == userRoleName)
                ?? throw new Exception();

            return userRole;
        }

        public async Task<List<Course>> GetUserCourses(string userId)
        {
            var user = await _appDbContext.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user.Courses.ToList();
        }
    }
    */
}
