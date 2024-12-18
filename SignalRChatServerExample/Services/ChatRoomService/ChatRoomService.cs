using Microsoft.EntityFrameworkCore;
using SignalRChatServerExample.Contexts;
using SignalRChatServerExample.DTOs.ChatRoomDTOs;
using SignalRChatServerExample.Entities;
using SignalRChatServerExample.Enums;
using SignalRChatServerExample.Services.UserService;

namespace SignalRChatServerExample.Services.ChatRoomService
{
    public class ChatRoomService(
        IUserService userService,
        ApplicationDbContext context) : IChatRoomService
    {
        public async Task<IEnumerable<string?>> CreateDirectChatAsync(string username)
        {
            ChatRoom chatRoom = new() { ChatRoomType = ChatRoomType.Direct, UpdatedDate = DateTime.UtcNow };
            chatRoom.Participants.Add(await userService.GetUserByUsernameAsync(userService.GetCurrentUsername));
            chatRoom.Participants.Add(await userService.GetUserByUsernameAsync(username));
            await context.ChatRooms.AddAsync(chatRoom);

            await context.SaveChangesAsync();

            return chatRoom.Participants.Where(p => !string.IsNullOrEmpty(p.ConnectionId)).Select(p => p.ConnectionId).ToList();
        }

        public async Task<IEnumerable<string?>> CreateGroupChatAsync(string name, string imageUrl, IEnumerable<string> usernameList)
        {
            ChatRoom chatRoom = new()
            {
                Name = name,
                ImageUrl = imageUrl,
                ChatRoomType = ChatRoomType.Group,
                UpdatedDate = DateTime.Now
            };
            chatRoom.Participants.Add(await userService.GetUserByUsernameAsync(userService.GetCurrentUsername));

            foreach (var username in usernameList)
                chatRoom.Participants.Add(await userService.GetUserByUsernameAsync(username));

            await context.ChatRooms.AddAsync(chatRoom);

            await context.SaveChangesAsync();

            return chatRoom.Participants.Where(p => !string.IsNullOrEmpty(p.ConnectionId)).Select(p => p.ConnectionId).ToList();
        }

        public async Task AddUserToGroupChatAsync(string username, string chatRoomId)
        {
            ChatRoom? chatRoom = await context.ChatRooms.FindAsync(Guid.Parse(chatRoomId));

            if (chatRoom is not null && chatRoom.ChatRoomType == ChatRoomType.Group)
            {
                AppUser? user = await userService.GetUserByUsernameAsync(username);
                if (user is not null)
                {
                    chatRoom.Participants.Add(user);
                    await context.SaveChangesAsync();

                }
            }
        }

        public async Task<IEnumerable<GetAllChatsDTO>> GetAllChatsAsync()
        {
            AppUser? user = await userService.GetUserByUsernameAsync(userService.GetCurrentUsername);

            if(user is not null)
            {
                int unreadMessageCount = await context.MessageReadStatuses
                    .Where(x => x.UserId == user.Id && !x.IsRead)
                    .CountAsync();

                return await context.ChatRooms
                    .Include(cr => cr.Participants)
                    .Include(cr => cr.Messages)
                    .ThenInclude(m => m.ReadStatuses)
                    .Where(cr => cr.Participants.Any(u => u.UserName == userService.GetCurrentUsername))
                    .OrderByDescending(cr => cr.UpdatedDate)
                    .Select(cr => new GetAllChatsDTO()
                    {
                        Id = cr.Id.ToString(),
                        Name = cr.ChatRoomType == ChatRoomType.Group ? cr.Name : cr.Participants.FirstOrDefault(u => u.UserName != user.UserName).NameSurname,
                        ImageUrl = cr.ChatRoomType == ChatRoomType.Group ? cr.ImageUrl : cr.Participants.FirstOrDefault(u => u.UserName != user.UserName).ImageUrl,
                        ChatRoomType = cr.ChatRoomType,
                        UpdatedDate = cr.UpdatedDate,
                        UnreadMessageCount = cr.Messages
                        .Where(m => m.ReadStatuses.Any(rs => !rs.IsRead && rs.UserId == user.Id))
                        .Count()
                    }).ToListAsync();
            }
            return null;
        }
    }
}
