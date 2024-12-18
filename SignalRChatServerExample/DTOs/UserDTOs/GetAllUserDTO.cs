namespace SignalRChatServerExample.DTOs.UserDTOs
{
    public class GetAllUserDTO
    {
        public string NameSurname { get; set; }
        public string? ImageUrl { get; set; }
        public string Username { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeenDate { get; set; }
    }
}
