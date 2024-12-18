namespace SignalRChatServerExample.Entities
{
    public class MessageReadStatus
    {
        public Guid MessageId { get; set; }
        public Guid UserId { get; set; }
        public bool IsRead { get; set; }
        public Message Message { get; set; }
        public AppUser User { get; set; }
    }
}
