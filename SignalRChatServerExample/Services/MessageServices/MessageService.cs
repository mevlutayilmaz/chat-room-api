using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                    ChatRoomId = chatRoom.Id,
                    Content = message,
                    SentAt = DateTime.Now,
                    Sender = await userService.GetUserByUsernameAsync(userService.GetCurrentUsername),
                };
                
                await context.Messages.AddAsync(_message);
                chatRoom.UpdatedDate = DateTime.Now;

                foreach (var user in chatRoom.Participants)
                    await context.MessageReadStatuses.AddAsync(new()
                    {
                        MessageId = _message.Id,
                        UserId = user.Id,
                        IsRead = user.UserName == userService.GetCurrentUsername ? true : false
                    });

                await context.SaveChangesAsync();

                return (new GetMessagesByChatId()
                {
                    Id = _message.Id.ToString(),
                    ChatRoomId = chatRoomId,
                    Content = _message.Content,
                    SenderName = _message.Sender.NameSurname,
                    SenderUsername = _message.Sender.UserName,
                    SentAt = _message.SentAt
                }, connectinIds);
            }
            return (null, null);
        }

        public async Task<IEnumerable<GetMessagesByChatId>> GetMessagesByChatIdAsync(string chatId)
        {
            await ChangeMessageReadStatusAsync(chatId);

            return await context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ChatRoomId == Guid.Parse(chatId))
                .OrderBy(m => m.SentAt)
                .Select(m => new GetMessagesByChatId()
                {
                    Id = m.Id.ToString(),
                    ChatRoomId = m.ChatRoomId.ToString(),
                    Content = m.Content,
                    SenderName = m.Sender.NameSurname,
                    SenderUsername = m.Sender.UserName,
                    SentAt = m.SentAt
                }).ToListAsync();
        }

        public async Task ChangeMessageReadStatusAsync(string chatId)
        {
            AppUser? user = await userService.GetUserByUsernameAsync(userService.GetCurrentUsername);

            if(user is not null)
            {
                await context.MessageReadStatuses
                    .Where(mrs => mrs.Message.ChatRoomId == Guid.Parse(chatId) && mrs.UserId == user.Id && !mrs.IsRead)
                    .ForEachAsync(mrs => mrs.IsRead = true);

                await context.SaveChangesAsync();
            }
        }
    }
}
