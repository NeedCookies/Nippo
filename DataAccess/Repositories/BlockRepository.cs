using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class BlockRepository(AppDbContext appDbContext) : IBlockRepository
    {
        public async Task<Block> Create(int lessonId, int type, string content)
        {
            var maxOrder = await GetMaxOrderByLessonId(lessonId);

            Block block = new Block
            {
                LessonId = lessonId,
                Type = (BlockType)type,
                Content = content,
                Order = maxOrder + 1
            };

            await appDbContext.Blocks.AddAsync(block);
            await appDbContext.SaveChangesAsync();

            return block;
        }

        public async Task<Block> Delete(int id)
        {
            var block = await GetByIdAsync(id);

            appDbContext.Blocks.Remove(block);
            await appDbContext.SaveChangesAsync();

            return block;
        }

        public async Task<Block> UpdateContent(int id, int type, string content)
        {
            var block = await GetByIdAsync(id);

            block.Type = (BlockType)type;
            block.Content = content;

            appDbContext.Blocks.Update(block);
            await appDbContext.SaveChangesAsync();

            return block;
        }

        public async Task<Block> UpdateOrder(int id, int order)
        {
            var block = await GetByIdAsync(id);

            block.Order = order;

            appDbContext.Blocks.Update(block);
            await appDbContext.SaveChangesAsync();

            return block;
        }

        public async Task<List<Block>> GetBlocksByLessonAsync(int lessonId) =>
            await appDbContext.Blocks
                .Where(b => b.LessonId == lessonId)
                .OrderBy(b => b.Order)
                .ToListAsync();

        public async Task<Block> GetByIdAsync(int id)
        {
            if (IsBlockExists(id) == false)
                throw new Exception("Block not exitsts");

            return await appDbContext.Blocks.FindAsync(id);
        }

        private bool IsBlockExists(int id) => 
            appDbContext.Blocks
                .Any(e => e.Id == id);

        private async Task<int> GetMaxOrderByLessonId(int lessonId) =>
            await appDbContext.Blocks
                .Where(b => b.LessonId == lessonId)
                .MaxAsync(b => (int?)b.Order) 
                ?? 0;
    }
}
