using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using System;
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

            StringBuilder error = new StringBuilder();
            if (lessonId < 0) { error.AppendLine("Wrong lesson id"); }
            if (type >= Enum.GetNames(typeof(BlockType)).Length || type < 0) 
                error.AppendLine("Bad content type");
            
            if (error.Length > 0) { throw new ArgumentException(error.ToString()); }

            return blockRepository.Create(lessonId, type, content);
        }

        public async Task<Block> Delete(int id)
        {
            return await blockRepository.Delete(id);
        }
        public async Task<Block> UpdateContent(UpdateBlockRequest request)
        {
            return await blockRepository.UpdateContent(request.Id, request.Type, request.Content);
        }

        public async Task<Block> GetById(int lessonId, int id)
        {
            return await blockRepository.GetByIdAsync(id);
        }

        public async Task<List<Block>> GetByLesson(int lessonId)
        {
            return await blockRepository.GetBlocksByLessonAsync(lessonId);
        }

        public async Task<Block> LowerBlockDown(int id)
        {
            var block = await blockRepository.GetByIdAsync(id);
            await MoveBlock(block, 1);
            return block;
        }

        public async Task<Block> RaiseBlockUp(int id)
        {
            var block = await blockRepository.GetByIdAsync(id);
            await MoveBlock(block, -1);
            return block;
        }

        private async Task MoveBlock(Block movableBlock, int newOrderOffset)
        {
            if (movableBlock == null)
                return;

            var blocks = await blockRepository.GetBlocksByLessonAsync(movableBlock.LessonId);
            int oldOrder = movableBlock.Order;
            int newOrder = oldOrder + newOrderOffset;
            int articleId = movableBlock.LessonId;

            var slidingBlock = blocks.FirstOrDefault(b => b.Order == newOrder);

            if (slidingBlock != null)
            {
                slidingBlock.Order = oldOrder;
                movableBlock.Order = newOrder;

                await blockRepository.UpdateOrder(slidingBlock.Id, slidingBlock.Order);
                await blockRepository.UpdateOrder(movableBlock.Id, movableBlock.Order);
            }
        }
    }
}
