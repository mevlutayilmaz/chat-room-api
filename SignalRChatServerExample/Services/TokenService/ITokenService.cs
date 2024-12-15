using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.Services.TokenService
{
    public interface ITokenService
    {
        TokenDTO CreateAccessToken(int second, AppUser user);
    }
}
