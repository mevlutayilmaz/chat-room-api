using SignalRChatServerExample.DTOs;

namespace SignalRChatServerExample.Services.MessageServices
{
    public interface IMessageService
    {
        public Task<IEnumerable<GetMessagesByChatId>> GetMessagesByChatIdAsync(string chatId);
        public Task<(GetMessagesByChatId, List<string?>)> CreateMessageAsync(string message, string chatRoomId);
        public Task ChangeMessageReadStatusAsync(string chatId);
    }
}
