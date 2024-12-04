using Application.Abstractions.Services;
using Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("admin")]
    [Authorize(Policy = "AdminRole")]
    public class AdminController(IUserService userService): ControllerBase
    {
        [HttpPost("give-points")]
        public async Task<IActionResult> GivePointsToUser(GivePointsRequest givePointsRequest)
        {
            var newUserInfo = await userService.GivePointsToUser(givePointsRequest.UserId, givePointsRequest.Points);

            return Ok(newUserInfo);
        }
    }
}
