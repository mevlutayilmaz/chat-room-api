using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRChatServerExample.Services.ChatRoomService;
using SignalRChatServerExample.Services.UserService;

namespace SignalRChatServerExample.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        IUserService userService,
        IChatRoomService chatRoomService) : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateDirectChat(string username)
        {
            await chatRoomService.CreateDirectChatAsync(username);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateGroupChat(string name, string imageUrl, List<string> usernameList)
        {
            await chatRoomService.CreateGroupChatAsync(name, imageUrl, usernameList);
            return Ok();
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> AddUserToGroupChat(string username, string chatRoomId)
        {
            await chatRoomService.AddUserToGroupChatAsync(username, chatRoomId);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
            => Ok(await userService.GetAllUsersAsync());

    }
}
