using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Contracts;
using Domain.Entities;
using System;
using System.Text;

namespace Application.Services
{
    public class BlockService(
        IBlockRepository blockRepository,
        IStorageService storageService) : IBlockService
    {
        public async Task<Block> Create(CreateBlockRequest request)
        {
            int lessonId = request.LessonId;
            int type = request.Type;

            string content = "";

            StringBuilder error = new StringBuilder();
            if (lessonId < 0) { error.AppendLine("Wrong lesson id"); }
            if (type >= Enum.GetNames(typeof(BlockType)).Length || type < 0)
                error.AppendLine("Bad content type");

            if (error.Length > 0) { throw new ArgumentException(error.ToString()); }

            if (BlockType.Text == (BlockType)type)
            {
                content = request.Content;
                return await blockRepository.Create(lessonId, type, content);
            }
            else
            {
                var pictureStream = request.Media.OpenReadStream();
                var fileName = Guid.NewGuid().ToString();

                await storageService.PutAsync(fileName, pictureStream, request.Media.ContentType);

                content = fileName;
                return await blockRepository.Create(lessonId, type, content);
            }
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
            var block = await blockRepository.GetByIdAsync(id);

            if (block == null || block.LessonId != lessonId)
                throw new ArgumentException("Block not found or does not belong to the specified lesson.");

            if (block.Type == BlockType.Image || block.Type == BlockType.Video)
            {
                var fileUrl = await storageService.GetUrlAsync(block.Content);
                block.Content = fileUrl;
            }

            return block;
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
