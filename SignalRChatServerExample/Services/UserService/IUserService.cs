﻿using SignalRChatServerExample.DTOs;
using SignalRChatServerExample.DTOs.UserDTOs;
using SignalRChatServerExample.Entities;

namespace SignalRChatServerExample.Services.UserService
{
    public interface IUserService
    {
        public string? GetCurrentUsername { get; }
        public AppUser? GetCurrentUser { get; }
        public Task OnConnectedAsync(string connectionId);
        public Task OnDisconnectedAsync();
        public Task<IEnumerable<GetAllUserDTO>> GetAllUsersAsync();
        public Task<AppUser?> GetUserByUsernameAsync(string username);
    }
}
