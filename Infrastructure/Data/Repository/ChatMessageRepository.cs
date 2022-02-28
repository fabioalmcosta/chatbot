using Domain.Entities;
using Infrastructure.Data.Context;
using Infrastructure.Data.Repository.Interfaces;

namespace Infrastructure.Data.Repository
{
    public class ChatMessageRepository : BaseRepository<ChatMessage>, IChatMessageRepository
    {
        public ChatMessageRepository(PostgresContext context) : base(context) { }
    }
}
