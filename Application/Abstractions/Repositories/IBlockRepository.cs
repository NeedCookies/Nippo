using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IBlockRepository
    {
        Task<List<Block>> GetBlocksByLessonAsync(int lessonId);
    }
}
