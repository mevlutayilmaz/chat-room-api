using SignalRChatServerExample.Enums;

namespace SignalRChatServerExample.DTOs.ChatRoomDTOs
{
    public class GetAllChatsDTO
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public ChatRoomType ChatRoomType { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UnreadMessageCount { get; set; }
    }
}
