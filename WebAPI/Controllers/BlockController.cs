using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("block")]
    public class BlockController(IBlockService blockService) : ControllerBase
    {
        [HttpGet("get-blocks-by-lesson")]
        public async Task<IActionResult> GetByLesson(int lessonId)
        {
            var blocks = await blockService.GetByLesson(lessonId);

            if (blocks == null)
            {
                return BadRequest("No blocks in this lesson");
            }

            return Ok(blocks);
        }

        [HttpGet("get-block-by-id")]
        public async Task<IActionResult> GetById(int lessonId, int id)
        {
            var block = await blockService.GetById(lessonId, id);

            if (block == null)
            {
                return BadRequest("No such block in this lesson");
            }

            return Ok(block);
        }

        [Authorize(Policy = "UpdateCourse")]
        [HttpPost("create-block")]
        public async Task<IActionResult> Create([FromForm] CreateBlockRequest request)
        {
            var block = await blockService.Create(request);
            return Ok(block);
        }

        [Authorize(Policy = "UpdateCourse")]
        [HttpPost("update-block")]
        public async Task<IActionResult> Update([FromBody] UpdateBlockRequest request)
        {
            var block = await blockService.UpdateContent(request);
            return Ok(block);
        }

        [Authorize(Policy = "UpdateCourse")]
        [HttpPost("delete-block")]
        public async Task<IActionResult> Delete(int id)
        {
            var block = await blockService.Delete(id);
            return Ok(block);
        }

        [Authorize(Policy = "UpdateCourse")]
        [HttpPost("move-up")]
        public async Task<IActionResult> RaiseBlockUp(int id)
        {
            var block = await blockService.RaiseBlockUp(id);
            return Ok(block);
        }

        [Authorize(Policy = "UpdateCourse")]
        [HttpPost("move-down")]
        public async Task<IActionResult> LowerBlockDown(int id)
        {
            var block = await blockService.LowerBlockDown(id);
            return Ok(block);
        }
    }
}
