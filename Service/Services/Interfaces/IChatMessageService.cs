using Domain.Entities;
using Domain.Models.ChatMessage;
using Domain.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task<UserChatConnection> AddConnection(string connectionId, string userEmail);
        Task<List<UserChatConnection>> GetConnectedUsers();
        Task Disconnect(string connectionId);
        Task<ChatMessage> PostMessage(string userId, ChatMessagePost message);
        Task<List<ChatMessage>> GetMessages(string userId, long toId);
    }
}
