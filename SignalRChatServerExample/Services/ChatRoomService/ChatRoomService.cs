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
        public async Task CreateDirectChatAsync(string username)
        {
            ChatRoom chatRoom = new() { ChatRoomType = ChatRoomType.Direct };
            chatRoom.Participants.Add(await userService.GetUserByUsernameAsync(userService.GetCurrentUsername));
            chatRoom.Participants.Add(await userService.GetUserByUsernameAsync(username));
            await context.ChatRooms.AddAsync(chatRoom);

            await context.SaveChangesAsync();
        }

        public async Task CreateGroupChatAsync(string name, string imageUrl, IEnumerable<string> usernameList)
        {
            ChatRoom chatRoom = new()
            {
                Name = name,
                ImageUrl = imageUrl,
                ChatRoomType = ChatRoomType.Group
            };
            chatRoom.Participants.Add(await userService.GetUserByUsernameAsync(userService.GetCurrentUsername));

            foreach (var username in usernameList)
                chatRoom.Participants.Add(await userService.GetUserByUsernameAsync(username));

            await context.ChatRooms.AddAsync(chatRoom);

            await context.SaveChangesAsync();
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
            if (userService.GetCurrentUsername is not null)
                return await context.ChatRooms
                    .Include(cr => cr.Participants)
                    .Where(cr => cr.Participants.Any(u => u.UserName == userService.GetCurrentUsername))
                    .Select(cr => new GetAllChatsDTO()
                    {
                        Id = cr.Id.ToString(),
                        Name = cr.ChatRoomType == ChatRoomType.Group ? cr.Name : cr.Participants.FirstOrDefault(u => u.UserName != userService.GetCurrentUsername).UserName,
                        ImageUrl = cr.ChatRoomType == ChatRoomType.Group ? cr.ImageUrl : cr.Participants.FirstOrDefault(u => u.UserName != userService.GetCurrentUsername).ImageUrl,
                        ChatRoomType = cr.ChatRoomType
                    }).ToListAsync();

            return null;
        }
    }
}
