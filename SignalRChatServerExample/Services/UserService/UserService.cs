using Microsoft.AspNetCore.Identity;
using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.Services.UserService
{
    public class UserService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor) : IUserService
    {
        public AppUser? GetCurrentUser => httpContextAccessor?.HttpContext?.User?.Identity?.Name is string userName
            ? userManager.FindByNameAsync(userName).Result
            : null;

        public async Task OnConnectedAsync(string connectionId)
        {
            AppUser? user = GetCurrentUser;
            if (user is not null)
            {
                user.ConnectionId = connectionId;
                user.IsOnline = true;
                IdentityResult result = await userManager.UpdateAsync(user);

            }
        }

        public async Task OnDisconnectedAsync()
        {
            AppUser? user = GetCurrentUser;
            if (user is not null)
            {
                user.ConnectionId = null;
                user.IsOnline = false;
                user.LastSeenDate = DateTime.UtcNow;
                IdentityResult result = await userManager.UpdateAsync(user);

            }
        }

    }
}
