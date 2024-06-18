using Application.Abstractions.Repositories;
using Domain.Entities;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    public class BlockRepository(AppDbContext dbContext) : IBlockRepository
    {
        public async Task<Block> Create(int lessonId, int type, string content, int order)
        {
            Block block = new Block
            {
                LessonId = lessonId,
                Type = (BlockType)type,
                Content = content,
                Order = order
            };

            await dbContext.Blocks.AddAsync(block);
            await dbContext.SaveChangesAsync();

            return block;
        }

        public async Task<List<Block>> GetBlocksByLessonAsync(int lessonId)
        {
            return await dbContext.Blocks.Where(b => b.LessonId == lessonId).ToListAsync();
        }

        public async Task<Block> GetByIdAsync(int lessonId, int id)
        {
            return await dbContext.Blocks.Where(b => b.LessonId == lessonId && b.Id == id).FirstAsync();
        }
    }
}
