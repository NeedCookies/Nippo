using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IBlockRepository
    {
        Task<List<Block>> GetBlocksByLessonAsync(int lessonId);
        Task<Block> GetByIdAsync(int lessonId, int id);
        Task<Block> Create(int lessonId, int type, string content, int order);
    }
}
