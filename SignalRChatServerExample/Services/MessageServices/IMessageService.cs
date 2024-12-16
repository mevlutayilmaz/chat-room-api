using SignalRChatServerExample.DTOs;

namespace SignalRChatServerExample.Services.MessageServices
{
    public interface IMessageService
    {
        public Task<IEnumerable<GetMessagesByChatId>> GetMessagesByChatIdAsync(string chatId);
    }
}
