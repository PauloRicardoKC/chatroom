using Chat.Domain.Interfaces.Application;
using Chat.Domain.Interfaces.Commands;
using MassTransit;

namespace Chat.UI.Consumers
{
    public class StockQuoteConsumer : IConsumer<IStockQuoteCommand>
    {
        private readonly ILogger<StockQuoteConsumer> _logger;
        private readonly IChatService _chatService;

        public StockQuoteConsumer(ILogger<StockQuoteConsumer> logger, IChatService chatService)
        {
            _logger = logger;
            _chatService = chatService; 
        }

        public async Task Consume(ConsumeContext<IStockQuoteCommand> context)
        {
            try
            {
                _logger.LogInformation($"Message: {context}");

                var message = context.Message;

                if (message == null) return;                

                //TODO ler arquivo, chamar serviço para retornar informação.
                //await _chatService.SendMessageAsync();                
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"[{nameof(StockQuoteConsumer)}] - Error processing message: {ex.Message}");
                throw;
            }
        }
    }
}