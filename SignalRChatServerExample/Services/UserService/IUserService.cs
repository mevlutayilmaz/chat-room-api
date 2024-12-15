using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.Services.UserService
{
    public interface IUserService
    {
        public AppUser? GetCurrentUser { get; }
        public Task OnConnectedAsync(string connectionId);
        public Task OnDisconnectedAsync();
    }
}
