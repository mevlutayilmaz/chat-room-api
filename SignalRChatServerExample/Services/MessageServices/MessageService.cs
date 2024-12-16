using Microsoft.EntityFrameworkCore;
using SignalRChatServerExample.Contexts;
using SignalRChatServerExample.DTOs;

namespace SignalRChatServerExample.Services.MessageServices
{
    public class MessageService(ApplicationDbContext context) : IMessageService
    {
        public async Task<IEnumerable<GetMessagesByChatId>> GetMessagesByChatIdAsync(string chatId)
            => await context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ChatRoomId == Guid.Parse(chatId))
                .OrderBy(m => m.SentAt)
                .Select(m => new GetMessagesByChatId()
                {
                    Id = m.Id.ToString(),
                    ChatRoomId = m.ChatRoomId.ToString(),
                    Content = m.Content,
                    IsRead = m.IsRead,
                    Sender = m.Sender.UserName,
                    SentAt = m.SentAt
                }).ToListAsync();
    }
}
