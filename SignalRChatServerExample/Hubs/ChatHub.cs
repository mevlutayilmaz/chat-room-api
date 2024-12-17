using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRChatServerExample.Contexts;
using SignalRChatServerExample.Entities;
using SignalRChatServerExample.Services.UserService;

namespace SignalRChatServerExample.Hubs
{
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ChatHub(IUserService userService, ApplicationDbContext context, UserManager<AppUser> userManager) : Hub
    {
        
        public override async Task OnConnectedAsync()
        {
            await userService.OnConnectedAsync(Context.ConnectionId);
            string? userName = userService.GetCurrentUsername;
            if (!string.IsNullOrEmpty(userName))
                await Clients.All.SendAsync("userConnected", userName);
        }
        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await userService.OnDisconnectedAsync();
            string? userName = userService.GetCurrentUsername;
            if (!string.IsNullOrEmpty(userName))
                await Clients.All.SendAsync("userDisconnected", userName);
        }
        
        public async Task SendMessageAsync(string message, string chatRoomId)
        {
            ChatRoom? chatRoom = await context.ChatRooms
                .Include(cr => cr.Participants)
                .Include(cr => cr.Messages)
                .FirstOrDefaultAsync(cr => cr.Id == Guid.Parse(chatRoomId));

            //AppUser sender = await userManager.Users.FirstOrDefaultAsync(u => u.ConnectionId == Context.ConnectionId);
            AppUser? sender = await userService.GetUserByUsernameAsync(userService.GetCurrentUsername);

            if (chatRoom is not null)
            {
                List<string?> connectinIds = chatRoom.Participants
                    .Where(u => !string.IsNullOrEmpty(u.ConnectionId))
                    .Select(u => u.ConnectionId)
                    .ToList();

                Message _message = new()
                {
                    Content = message,
                    IsRead = false,
                    SenderId = sender.Id,
                    SentAt = DateTime.UtcNow,
                    ChatRoomId = Guid.Parse(chatRoomId)
                };
                chatRoom.Messages.Add(_message);

                await Clients.Clients(connectinIds).SendAsync("receiveMessage", _message, chatRoom.Id);
            }

            await context.SaveChangesAsync();

        }

        //public async Task GetNickNameAsync(string nickName)
        //{
        //    Client client = new()
        //    {
        //        ConnectionId = Context.ConnectionId,
        //        NickName = nickName
        //    };

        //    ClientSource.Clients.Add(client);
        //    await Clients.Others.SendAsync("clientJoined", nickName);
        //    await Clients.All.SendAsync("clients", ClientSource.Clients);
        //}

        //public async Task SendMessageAsync(string message, string clientName)
        //{
        //    Client client = ClientSource.Clients.FirstOrDefault(c => c.NickName == clientName.Trim());
        //    Client senderClient = ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
        //    await Clients.Client(client.ConnectionId).SendAsync("receiveMessage", message, senderClient.NickName);
        //}

        //public async Task AddGroupAsync(string groupName)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        //    Group group = new() { GroupName = groupName };
        //    group.Clients.Add(ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId));

        //    GroupSource.Groups.Add(group);

        //    await Clients.All.SendAsync("groups", GroupSource.Groups);
        //}

        //public async Task AddClientToGroupAsync(IEnumerable<string> groupNames)
        //{
        //    Client client = ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
        //    foreach (var groupName in groupNames)
        //    {
        //        Group group = GroupSource.Groups.FirstOrDefault(g => g.GroupName == groupName);
        //        group.Clients.Add(client);
        //        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        //    }
        //}
    }
}
