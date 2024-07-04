using Application.Abstractions.Repositories;

namespace DataAccess.Repositories
{
    public class UnitOfWork(AppDbContext appDbContext) : IUnitOfWork
    {
        public async Task SaveChangesAsync()
        {
            await appDbContext.SaveChangesAsync();
        }
    }
}
