using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.DTOs.UserDTOs;
using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.Services.UserService
{
    public class UserService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor) : IUserService
    {
        public string? GetCurrentUsername => httpContextAccessor?.HttpContext?.User?.Identity?.Name;

        public async Task OnConnectedAsync(string connectionId)
        {
            AppUser? user = await GetUserByUsernameAsync(GetCurrentUsername);
            if (user is not null)
            {
                user.ConnectionId = connectionId;
                user.IsOnline = true;
                IdentityResult result = await userManager.UpdateAsync(user);

            }
        }

        public async Task OnDisconnectedAsync()
        {
            AppUser? user = await GetUserByUsernameAsync(GetCurrentUsername);
            if (user is not null)
            {
                user.ConnectionId = null;
                user.IsOnline = false;
                user.LastSeenDate = DateTime.UtcNow;
                IdentityResult result = await userManager.UpdateAsync(user);

            }
        }

        public async Task<IEnumerable<GetAllUserDTO>> GetAllUsersAsync()
            => await userManager.Users
            .Where(u => u.UserName != GetCurrentUsername)
            .Select(user => new GetAllUserDTO()
            {
                Id = user.Id,
                Username = user.UserName,
                ImageUrl = user.ImageUrl,
                IsOnline = user.IsOnline,
                LastSeenDate = DateTime.UtcNow
            }).ToListAsync();

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
            => !string.IsNullOrEmpty(username) ? await userManager.FindByNameAsync(username) : null;

    }
}
