namespace Application.Abstractions.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
