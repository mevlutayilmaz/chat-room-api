using Microsoft.AspNetCore.Mvc;
using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.DTOs.AuthDTO;
using SignalRChatServerExample.Services.AuthService;

namespace SignalRChatServerExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        IAuthService authService) : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            bool result = await authService.RegisterAsync(request);

            if (result) return Ok();
            else return BadRequest();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            TokenDTO token= await authService.LoginAsync(request);

            return Ok(token);
        }
    }
}
