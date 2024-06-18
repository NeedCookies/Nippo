using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using System.Text;

namespace Application.Services
{
    public class BlockService(IBlockRepository blockRepository) : IBlockService
    {
        public Task<Block> Create(CreateBlockRequest request)
        {
            int lessonId = request.LessonId;
            int type = request.Type;
            string content = request.Content;
            int order = request.Order;

            StringBuilder error = new StringBuilder();
            if (lessonId < 0) { error.AppendLine("Wrong lesson id"); }
            if (type >= Enum.GetNames(typeof(BlockType)).Length || type < 0) 
                error.AppendLine("Bad content type");
            
            if (error.Length > 0) { throw new ArgumentException(error.ToString()); }

            return blockRepository.Create(lessonId, type, content, order);
        }

        public async Task<Block> GetById(int lessonId, int id)
        {
            return await blockRepository.GetByIdAsync(lessonId, id);
        }

        public async Task<List<Block>> GetByLesson(int lessonId)
        {
            return await blockRepository.GetBlocksByLessonAsync(lessonId);
        }
    }
}
