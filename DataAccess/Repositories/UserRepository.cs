using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<ApplicationUser> Add(Guid userId,
            string firstName, string lastName,
            string userName, string email,
            DateOnly birthDate, string phoneNumber,
            AppRole role, int points)
        {
            var id = Guid.NewGuid().ToString();

            var user = new ApplicationUser()
            {
                Id = userId,
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                Role = role,
                BirthDate = birthDate,
                PhoneNumber = phoneNumber,
                Points = points
            };

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser?> GetByUserId(Guid userId) =>
            await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

        public async Task<ApplicationUser?> GetByUserName(string userName) =>
            await _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == userName);
        
        public async Task<ApplicationUser?> GetByEmail(string email) => 
            await _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        public async Task<AppRole> GetUserRole(Guid userId)
        {
            var user = await _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NullReferenceException($"There's no user with id: {userId}");

            return user.Role;
        }

        public async Task<List<Course>> GetUserCourses(Guid userId)
        {
            var user = await _appDbContext.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NullReferenceException($"There's no user with id: {userId}");

            return user.Courses.ToList();
        }

        public async Task<List<ApplicationUser>> GetAllUsers() =>
            await _appDbContext.Users.ToListAsync();
    }
}
