using Microsoft.EntityFrameworkCore;
using SignalRChatServerExample.Contexts;
using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.Entities;
using SignalRChatServerExample.Services.UserService;
using System.Reflection;

namespace SignalRChatServerExample.Services.MessageServices
{
    public class MessageService(ApplicationDbContext context, IUserService userService) : IMessageService
    {
        public async Task<(GetMessagesByChatId, List<string?>)> CreateMessageAsync(string message, string chatRoomId)
        {
            ChatRoom? chatRoom = await context.ChatRooms
                .Include(cr => cr.Participants)
                .FirstOrDefaultAsync(cr => cr.Id == Guid.Parse(chatRoomId));

            if (chatRoom is not null)
            {
                List<string?> connectinIds = chatRoom.Participants
                    .Where(u => !string.IsNullOrEmpty(u.ConnectionId))
                    .Select(u => u.ConnectionId)
                    .ToList();

                Message _message = new()
                {
                    Content = message,
                    SentAt = DateTime.UtcNow,
                    Sender = await userService.GetUserByUsernameAsync(userService.GetCurrentUsername),
                    IsRead = false
                };

                chatRoom.Messages.Add(_message);

                await context.SaveChangesAsync();

                return (new GetMessagesByChatId()
                {
                    Id = _message.Id.ToString(),
                    ChatRoomId = chatRoomId,
                    Content = _message.Content,
                    IsRead = false,
                    Sender = _message.Sender.UserName,
                    SentAt = _message.SentAt
                }, connectinIds);
            }
            return (null, null);
        }

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
