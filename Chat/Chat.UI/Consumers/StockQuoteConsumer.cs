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
               
                using (var client = new HttpClient())
                {
                    var stockCode = message.StockCode.Split('=')[1];

                    var uri = $"https://stooq.com/q/l/?s={stockCode}";

                    var request = new HttpRequestMessage(HttpMethod.Get, uri);

                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = await response.Content.ReadAsStringAsync();

                        if (responseStream.Split(',')[3] != "N/D")
                        {
                            var stockQuote = responseStream.Split(',')[3];

                            var messageFormat = $"{stockCode} quote is ${stockQuote} per share";

                            await _chatService.SendMessageAsync(message.HubCallerContext, message.Clients, messageFormat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"[{nameof(StockQuoteConsumer)}] - Error processing message: {ex.Message}");
                throw;
            }
        }
    }
}