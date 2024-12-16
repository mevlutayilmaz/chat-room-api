using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    }
}
