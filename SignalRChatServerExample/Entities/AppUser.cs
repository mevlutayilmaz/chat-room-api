using Microsoft.AspNetCore.Identity;
using SignalRChatServerExample.Hubs;

namespace SignalRChatServerExample.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? ConnectionId { get; set; }
        public UserStatus UserStatus { get; set; }
        public ICollection<ChatRoom> ChatRooms { get; set; } = new HashSet<ChatRoom>();
    }
}
