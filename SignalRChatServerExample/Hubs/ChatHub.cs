using Microsoft.AspNetCore.SignalR;
using SignalRChatServerExample.Entities;
using SignalRChatServerExample.Services.UserService;

namespace SignalRChatServerExample.Hubs
{
    public class ChatHub(IUserService userService) : Hub
    {
        public override async Task OnConnectedAsync()
            => await userService.OnConnectedAsync(Context.ConnectionId);

        public override async Task OnDisconnectedAsync(Exception? exception)
            => await userService.OnDisconnectedAsync();

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
