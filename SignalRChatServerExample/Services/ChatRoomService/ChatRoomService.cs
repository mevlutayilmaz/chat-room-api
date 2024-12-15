using SignalRChatServerExample.Contexts;
using SignalRChatServerExample.Entities;
using SignalRChatServerExample.Services.UserService;

namespace SignalRChatServerExample.Services.ChatRoomService
{
    public class ChatRoomService(
        IUserService userService,
        ApplicationDbContext context) : IChatRoomService
    {
        public async Task CreateDirectChatAsync(string username)
        {
            ChatRoom chatRoom = new() { ChatRoomType = Enums.ChatRoomType.Direct };
            chatRoom.Participants.Add(userService.GetCurrentUser);
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
                ChatRoomType = Enums.ChatRoomType.Group
            };
            chatRoom.Participants.Add(userService.GetCurrentUser);

            foreach (var username in usernameList)
                chatRoom.Participants.Add(await userService.GetUserByUsernameAsync(username));

            await context.ChatRooms.AddAsync(chatRoom);

            await context.SaveChangesAsync();
        }
    }
}
