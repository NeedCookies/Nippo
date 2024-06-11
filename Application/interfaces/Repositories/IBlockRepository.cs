using Domain.Entities;

namespace Application.interfaces.Repositories
{
    public interface IBlockRepository
    {
        Task<List<Block>> GetBlocksByLessonAsync(int lessonId);
    }
}
