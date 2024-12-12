using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("tests")]
    public class TestControllers : ControllerBase
    {
        [HttpGet("public")]
        public async Task<IActionResult> getPublic()
        {
            return Ok("public good");
        }

        [HttpGet("create-course")]
        [Authorize(Policy ="CreateCourse")]
        public async Task<IActionResult> createCourse()
        {
            return Ok("Course created");
        }

        [HttpGet("create-promo")]
        [Authorize(Policy ="CreatePromo")]
        public async Task<IActionResult> createPromo()
        {
            return Ok("Promo created");
        }

        [HttpGet("test-admin-role")]
        [Authorize(Policy = "AdminRole")]
        public async Task<IActionResult> testAdmin()
        {
            return Ok("Yes, you have admin's role");
        }
    }
}
