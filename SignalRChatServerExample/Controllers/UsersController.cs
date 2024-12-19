using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRChatServerExample.Services.UserService;

namespace SignalRChatServerExample.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        IUserService userService) : ControllerBase
    {

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
            => Ok(await userService.GetAllUsersAsync());

        [HttpGet("[action]/{chatRoomId}")]
        public async Task<IActionResult> GetUserOnlineStatus(string chatRoomId)
            => Ok(await userService.GetUserOnlineStatusAsync(chatRoomId));

    }
}
