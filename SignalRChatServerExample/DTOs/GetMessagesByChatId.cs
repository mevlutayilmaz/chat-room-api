using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.DTOs
{
    public class GetMessagesByChatId
    {
        public string Id { get; set; }
        public string ChatRoomId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public string Sender { get; set; }
    }
}
