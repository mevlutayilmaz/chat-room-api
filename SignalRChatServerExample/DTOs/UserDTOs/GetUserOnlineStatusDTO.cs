namespace SignalRChatServerExample.DTOs.UserDTOs
{
    public class GetUserOnlineStatusDTO
    {
        public string NameSurname { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeenDate { get; set; }
    }
}
