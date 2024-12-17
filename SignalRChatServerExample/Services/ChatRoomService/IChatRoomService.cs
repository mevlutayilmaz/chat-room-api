using SignalRChatServerExample.DTOs.ChatRoomDTOs;

namespace SignalRChatServerExample.Services.ChatRoomService
{
    public interface IChatRoomService
    {
        public Task<IEnumerable<GetAllChatsDTO>> GetAllChatsAsync();
        public Task<IEnumerable<string?>> CreateDirectChatAsync(string username);
        public Task<IEnumerable<string?>> CreateGroupChatAsync(string name, string imageUrl, IEnumerable<string> usernameList);
        public Task AddUserToGroupChatAsync(string username, string chatRoomId);
    }
}
