using Chat.Domain.Entities;
using Chat.Domain.Interfaces.Persistence;
using Chat.Infra.Data.DataBases;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<ChatMessage>> GetLastMessagesAsync()
        {
            return await _context.Set<ChatMessage>()
                .FromSqlRaw("SELECT TOP(50) c.*, u.Username FROM ChatMessage c " +
                            "inner join AspNetUsers u on (c.SenderUserId = u.Id) " +
                            "order by c.SentDate desc")
                .ToListAsync();                
        }
    }
}