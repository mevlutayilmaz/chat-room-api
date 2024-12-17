using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRChatServerExample.Services.ChatRoomService;

namespace SignalRChatServerExample.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomsController(IChatRoomService chatRoomService) : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllChats()
            => Ok(await chatRoomService.GetAllChatsAsync());

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateDirectChat(string username)
        {
            await chatRoomService.CreateDirectChatAsync(username);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateGroupChat(string name, string imageUrl, [FromBody] List<string> usernameList)
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
    }
}
