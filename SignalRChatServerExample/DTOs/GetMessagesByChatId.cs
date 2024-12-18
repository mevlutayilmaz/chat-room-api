using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.DTOs
{
    public class GetMessagesByChatId
    {
        public string Id { get; set; }
        public string ChatRoomId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public string SenderName { get; set; }
        public string SenderUsername { get; set; }
    }
}
