namespace SignalRChatServerExample.Services.ChatRoomService
{
    public interface IChatRoomService
    {
        public Task CreateDirectChatAsync(string username);
        public Task CreateGroupChatAsync(string name, string imageUrl, IEnumerable<string> usernameList);
    }
}
