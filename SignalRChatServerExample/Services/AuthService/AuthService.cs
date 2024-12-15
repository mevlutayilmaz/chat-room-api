using Azure.Core;
using Microsoft.AspNetCore.Identity;
using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.DTOs.AuthDTO;
using SignalRChatServerExample.Entities;
using SignalRChatServerExample.Services.TokenService;

namespace SignalRChatServerExample.Services.AuthService
{
    public class AuthService(
        UserManager<AppUser> userManager, 
        SignInManager<AppUser> signInManager,
        ITokenService tokenService) : IAuthService
    {
        public async Task<TokenDTO> LoginAsync(LoginDTO loginDTO)
        {
            AppUser? user = await userManager.FindByNameAsync(loginDTO.UsernameOrEmail);
            if (user is null)
                user = await userManager.FindByEmailAsync(loginDTO.UsernameOrEmail);
            if (user is null)
                throw new Exception("User not found!");


            var result = await signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (result.Succeeded)
                return tokenService.CreateAccessToken(36000, user);

            throw new Exception("Authentication error!");
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDTO)
        {
            IdentityResult result = await userManager.CreateAsync(new()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Username,
            }, registerDTO.Password);

            return result.Succeeded;
        }
    }
}
