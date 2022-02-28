using Domain.Entities;
using Domain.Models.ChatMessage;
using Domain.Models.User;
using Infrastructure.Data.Repository.Interfaces;
using Infrastructure.Exception;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Hubs;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private IChatMessageRepository _chatMessageRepository;
        private IUserRepository _userRepository;

        public ChatMessageService(IHubContext<ChatHub> chatHub, IChatMessageRepository chatMessageRepository, IUserRepository userRepository)
        {
            _chatHub = chatHub;
            _chatMessageRepository = chatMessageRepository;
            _userRepository = userRepository;
        }

        public async Task<UserChatConnection> AddConnection(string connectionId, string userEmail)
        {
            var user = await _userRepository.GetAllReadOnly().Where(x => x.Email == userEmail).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "User not found!");
            }

            user.ConnectionHost = connectionId;
            user.ConnectionDate = DateTime.Now;

            _userRepository.Update(user);

            return new UserChatConnection()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                ConnectionDate = user.ConnectionDate,
                ConnectionHost = user.ConnectionHost,
                Email = user.Email
            };
        }

        public async Task<List<UserChatConnection>> GetConnectedUsers()
        {
            var connectedUsers = await _userRepository.GetAllReadOnly().Where(x => x.ConnectionHost != null)
                .Select(x => new UserChatConnection()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    ConnectionDate = x.ConnectionDate,
                    ConnectionHost = x.ConnectionHost,
                    Email = x.Email
                })
                .ToListAsync();

            return connectedUsers;
        }

        public async Task Disconnect(string connectionId)
        {
            var user = await _userRepository.GetAllReadOnly().Where(x => x.ConnectionHost == connectionId).FirstOrDefaultAsync();

            if (user != null)
            {
                user.ConnectionDate = null;
                user.ConnectionHost = null;

                _userRepository.Update(user);
            }
        }

        public async Task<ChatMessage> PostMessage(string userId, ChatMessagePost message)
        {
            var user = await _userRepository.GetByIdAsync(Int64.Parse(userId));

            if (user != null)
            {
                if (message.Message.StartsWith("/stock="))
                {
                    var chatMessageStock = await FindStock(user, message);
                    return chatMessageStock;
                }
                else
                {
                    var toUser = await _userRepository.GetByIdAsync(message.ToId);
                    var chatMessage = new ChatMessage()
                    {
                        Date = DateTime.Now,
                        ToId = message.ToId,
                        FromId = user.Id,
                        Message = message.Message
                    };

                    _chatMessageRepository.Insert(chatMessage);

                    if (toUser.ConnectionHost != null)
                    {
                        await _chatHub.Clients.Client(toUser.ConnectionHost).SendAsync("Receive", user.Id, chatMessage);
                    }

                    return chatMessage;
                }

            }
            else
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "User not found!");
            }
        }

        private async Task<ChatMessage> FindStock(User user, ChatMessagePost message)
        {
            List<string> splitted = new List<string>();
            string fileList = GetCSV(String.Format("https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv", message.Message.Remove(0, 7)));
            string[] tempStr;

            tempStr = fileList.Split(',');

            foreach (string item in tempStr)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    splitted.Add(item);
                }
            }

            var chatMessage = new ChatMessage()
            {
                Date = DateTime.Now,
                ToId = 0,
                FromId = user.Id,
                Message = "The Price of " + splitted[7].Remove(0, 8) + " is: $" + splitted[13]
            };

            return chatMessage;
        }

        private string GetCSV(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
        }

        public async Task<List<ChatMessage>> GetMessages(string userId, long toId)
        {
            var chatList = await _chatMessageRepository.GetAllReadOnly()
                .Where(x => (x.FromId == Int64.Parse(userId) && x.ToId == toId) || (x.ToId == Int64.Parse(userId) && x.FromId == toId))
                .OrderByDescending(x => x.Date)
                .Take(50)
                .ToListAsync();

            chatList.Reverse();

            return chatList;
        }
    }
}
