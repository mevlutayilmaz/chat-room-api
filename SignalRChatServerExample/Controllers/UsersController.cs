using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRChatServerExample.Services.UserService;

namespace SignalRChatServerExample.Controllers
{
    [Authorize(AuthenticationSchemes = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            
            return Ok(userService.GetCurrentUser);
        }

    }
}
