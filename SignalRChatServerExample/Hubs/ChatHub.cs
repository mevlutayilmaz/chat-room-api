using Microsoft.AspNetCore.SignalR;
using SignalRChatServerExample.Data;
using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.Hubs
{
    public class ChatHub : Hub
    {
        public async Task GetNickNameAsync(string nickName)
        {
            Client client = new()
            {
                ConnectionId = Context.ConnectionId,
                NickName = nickName
            };

            ClientSource.Clients.Add(client);
            await Clients.Others.SendAsync("clientJoined", nickName);
            await Clients.All.SendAsync("clients", ClientSource.Clients);
        }

        public async Task SendMessageAsync(string message, string clientName)
        {
            Client client = ClientSource.Clients.FirstOrDefault(c => c.NickName == clientName.Trim());
            await Clients.Client(client.ConnectionId).SendAsync("receiveMessage", message);
        }
    }
}
