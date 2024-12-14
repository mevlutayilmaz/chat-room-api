using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SignalRChatServerExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("test")]
        [Authorize] // Sadece kimliği doğrulanmış kullanıcılara izin ver
        public IActionResult Test()
        {
            var username = HttpContext.User.Identity.Name; // kullanıcının adını al
            return Ok($"Merhaba, {username}!");
        }

    }
}
