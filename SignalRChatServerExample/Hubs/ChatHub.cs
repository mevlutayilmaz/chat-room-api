using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.Entities;
using SignalRChatServerExample.Services.ChatRoomService;
using SignalRChatServerExample.Services.MessageServices;
using SignalRChatServerExample.Services.UserService;

namespace SignalRChatServerExample.Hubs
{
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ChatHub(
        IUserService userService,
        IMessageService messageService,
        IChatRoomService chatRoomService) : Hub
    {
        
        public override async Task OnConnectedAsync()
        {
            await userService.OnConnectedAsync(Context.ConnectionId);
            string? username = userService.GetCurrentUsername;
            if (!string.IsNullOrEmpty(username))
                await Clients.All.SendAsync("userConnected", username);
        }
        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await userService.OnDisconnectedAsync();
            string? username = userService.GetCurrentUsername;
            if (!string.IsNullOrEmpty(username))
                await Clients.All.SendAsync("userDisconnected", username);
        }
        
        public async Task SendMessageAsync(string message, string chatRoomId)
        {
            (GetMessagesByChatId _message, List<string?> connectionIds) = await messageService.CreateMessageAsync(message, chatRoomId);

            if (_message != null && connectionIds != null)
                await Clients.Clients(connectionIds).SendAsync("receiveMessage", _message, _message.ChatRoomId);
        }

        public async Task CreateDirectChatAsync(string username)
        {
            var connectionIds = await chatRoomService.CreateDirectChatAsync(username);
            if(connectionIds.Any())
                await Clients.Clients(connectionIds).SendAsync("chatRoomCreated");
        }

        public async Task CreateGroupChatAsync(string name, string imageUrl, IEnumerable<string> usernameList)
        {
            var connectionIds = await chatRoomService.CreateGroupChatAsync(name, imageUrl, usernameList);
            if (connectionIds.Any())
                await Clients.Clients(connectionIds).SendAsync("chatRoomCreated");
        }

    }
}
