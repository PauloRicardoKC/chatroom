using Chat.Domain.Entities;

namespace Chat.Domain.Interfaces.Persistence
{
    public interface IChatRepository
    {
        Task SaveAsync(ChatMessage entity);
    }
}