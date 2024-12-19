using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRChatServerExample.Contexts;
using SignalRChatServerExample.DTOs.UserDTOs;
using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.Services.UserService
{
    public class UserService(
        UserManager<AppUser> userManager, 
        IHttpContextAccessor httpContextAccessor, 
        ApplicationDbContext context) : IUserService
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
                user.LastSeenDate = DateTime.Now;
                IdentityResult result = await userManager.UpdateAsync(user);

            }
        }

        public async Task<IEnumerable<GetAllUserDTO>> GetAllUsersAsync()
            => await userManager.Users
            .Where(u => u.UserName != GetCurrentUsername)
            .Select(user => new GetAllUserDTO()
            {
                NameSurname = user.NameSurname,
                Username = user.UserName,
                ImageUrl = user.ImageUrl,
                IsOnline = user.IsOnline,
                LastSeenDate = user.LastSeenDate
            }).ToListAsync();

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
            => !string.IsNullOrEmpty(username) ? await userManager.FindByNameAsync(username) : null;

        public async Task<IEnumerable<GetUserOnlineStatusDTO>> GetUserOnlineStatusAsync(string chatRoomId)
            => await context.ChatRooms
                .Include(cr => cr.Participants)
                .Where(cr => cr.Id == Guid.Parse(chatRoomId))
                .SelectMany(cr => cr.Participants.Where(p => p.UserName != GetCurrentUsername))
                .Select(p => new GetUserOnlineStatusDTO()
                {
                    NameSurname = p.NameSurname,
                    IsOnline = p.IsOnline,
                    LastSeenDate = p.LastSeenDate
                }).ToListAsync();
    }
}
