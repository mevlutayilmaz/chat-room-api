using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRChatServerExample.Services.MessageServices;

namespace SignalRChatServerExample.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController(IMessageService messageService) : ControllerBase
    {
        [HttpGet("[action]/{chatId}")]
        public async Task<IActionResult> GetMessagesByChatId(string chatId)
            => Ok(await messageService.GetMessagesByChatIdAsync(chatId));
    }
}
