using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        SignInManager<AppUser> signInManager, 
        UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            IdentityResult result = await userManager.CreateAsync(new()
            {
                Email = request.Email,
                UserName = request.Username
            }, request.Password);


            if (result.Succeeded) return Ok();
            else return BadRequest();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            AppUser? user = await userManager.FindByNameAsync(request.UsernameOrEmail);
            if (user is null)
                user = await userManager.FindByEmailAsync(request.UsernameOrEmail);
            if (user is null)
                return NotFound();


            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded) return Ok();
            else return BadRequest();
        }
    }
}
