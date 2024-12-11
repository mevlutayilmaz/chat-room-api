using SignalRChatServerExample.Enums;

namespace SignalRChatServerExample.Entities
{
    public class ChatRoom
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public ChatRoomType ChatRoomType { get; set; }
        public ICollection<AppUser> Participants { get; set; } = new HashSet<AppUser>();
        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
