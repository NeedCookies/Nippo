using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IBlockRepository
    {
        Task<List<Block>> GetBlocksByLessonAsync(int lessonId);
        Task<Block> GetByIdAsync(int id);
        Task<Block> Create(int lessonId, int type, string content);
        Task<Block> UpdateContent(int id, int type, string content);
        Task<Block> UpdateOrder(int id, int order);
        Task<Block> Delete (int id);
    }
}
