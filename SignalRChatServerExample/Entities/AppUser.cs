using Microsoft.AspNetCore.Identity;

namespace SignalRChatServerExample.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? ConnectionId { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeenDate { get; set; }
        public ICollection<ChatRoom> ChatRooms { get; set; } = new HashSet<ChatRoom>();
        public ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();
    }
}
