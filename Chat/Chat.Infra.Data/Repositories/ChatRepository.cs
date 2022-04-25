using Chat.Domain.Entities;
using Chat.Domain.Interfaces.Persistence;
using Chat.Infra.Data.DataBases.Context;
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
            try
            {
                await _context.Set<ChatMessage>().AddAsync(entity);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<IEnumerable<ChatMessage>> GetLastMessagesAsync()
        {
            var response = await _context.Set<ChatMessage>()
                .FromSqlRaw("SELECT TOP(50) c.MessageId, c.SenderUserId, c.Message, c.SentDate, u.Username FROM ChatMessage c " +
                            "inner join AspNetUsers u on (c.SenderUserId = u.Id) " +
                            "order by c.SentDate desc")
                .ToListAsync();

            return response;
        }
    }
}