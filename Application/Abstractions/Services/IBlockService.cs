using Application.Contracts;
using Domain.Entities;

namespace Application.Abstractions.Services
{
    public interface IBlockService
    {
        Task<List<Block>> GetByLesson(int lessonId);
        Task<Block> Create(CreateBlockRequest request);
        Task<Block> GetById(int lessonId, int id);
    }
}
