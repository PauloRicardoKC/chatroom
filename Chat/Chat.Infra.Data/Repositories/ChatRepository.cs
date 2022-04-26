using Chat.Domain.Entities;
using Chat.Domain.Interfaces.Persistence;
using Chat.Infra.Data.DataBases.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Chat.Infra.Data.Repositories
{
    [ExcludeFromCodeCoverage]
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
                .FromSqlRaw("SELECT TOP(50) c.MessageId, c.SenderUserId, c.Message, c.SentDate, u.Username FROM ChatMessage c " +
                            "inner join AspNetUsers u on (c.SenderUserId = u.Id) " +
                            "order by c.SentDate desc")
                .ToListAsync();
        }
    }
}