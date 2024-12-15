using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.DTOs.AuthDTO;

namespace SignalRChatServerExample.Services.AuthService
{
    public interface IAuthService
    {
        public Task<TokenDTO> LoginAsync(LoginDTO loginDTO);
        public Task<bool> RegisterAsync(RegisterDTO registerDTO);
    }
}
