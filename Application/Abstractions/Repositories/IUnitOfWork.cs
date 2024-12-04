namespace Application.Abstractions.Repositories
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves changes to db
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
