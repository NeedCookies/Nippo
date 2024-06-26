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

        [Authorize(Roles = "admin")]
        [HttpPost("create-block")]
        public async Task<IActionResult> Create([FromBody] CreateBlockRequest request)
        {
            var block = await blockService.Create(request);
            return Ok(block); 
        }
    }
}
