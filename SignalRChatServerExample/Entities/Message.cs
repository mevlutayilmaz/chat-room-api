namespace SignalRChatServerExample.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ChatRoomId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public ChatRoom ChatRoom { get; set; }
        public AppUser Sender { get; set; }
    }
}
