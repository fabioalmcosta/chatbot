using Domain.Entities;
using Domain.Models.User;
using Microsoft.AspNetCore.SignalR;
using Service.Hubs.Interfaces;
using Service.Services.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Hubs
{
    public class ChatHub : Hub
    {
        private IChatMessageService _appService;
        private IUserService _userService;

        public ChatHub(IChatMessageService appService, IUserService userService)
        {
            _appService = appService;
            _userService = userService;
        }

        /// <summary>
        /// Override para inserir cada usuário no nosso repositório, lembrando que esse repositório está em memória
        /// </summary>
        /// <returns> Retorna lista de usuário no chat e usuário que acabou de logar </returns>
        public async override Task<Task> OnConnectedAsync()
        {
            var userQuery = JsonSerializer.Deserialize<string>(Context.GetHttpContext().Request.Query["user"]);
            var user = await _appService.AddConnection(Context.ConnectionId, userQuery);
            //Ao usar o método All eu estou enviando a mensagem para todos os usuários conectados no meu Hub
            await Clients.All.SendAsync("chat", await _appService.GetConnectedUsers(), user);
            return base.OnConnectedAsync();
        }

        public async override Task<Task> OnDisconnectedAsync(System.Exception exception)
        {
            await _appService.Disconnect(Context.ConnectionId);
            await Clients.All.SendAsync("chat", await _appService.GetConnectedUsers());

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Método responsável por encaminhar as mensagens pelo hub
        /// </summary>
        /// <param name="ChatMessage">Este parâmetro é nosso objeto representando a mensagem e os usuários envolvidos</param>
        /// <returns></returns>
        public async Task SendMessage(ChatMessage chat, string connection)
        {
            //if (chat.toId.Equals(publicId))
            //{
            //    await Clients.All.SendAsync("Public", chat.from, chat.message);
            //    return;
            //}
            //var user = await _userService.Get(chat.ToId);
            //var connection = user.ConnectionHost;
            await Clients.Client(connection).SendAsync("Receive", chat.FromId, chat);
        }
    }
}
