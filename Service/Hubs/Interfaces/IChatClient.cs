using Domain.Entities;
using System.Threading.Tasks;

namespace Service.Hubs.Interfaces
{
    public interface IChatClient
    {
        Task ReceiveMessage(ChatMessage message);
    }
}
