using Chat.Domain.Entities;
using Chat.Domain.Interfaces.Persistence;
using Chat.Infra.Data.DataBases;

namespace Chat.Infra.Data.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;

        public ChatRepository(ApplicationDbContext context)
        {           
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SaveAsync(ChatMessage entity)
        {
            await _context.Set<ChatMessage>().AddAsync(entity);

            await _context.SaveChangesAsync();
        }
    }
}