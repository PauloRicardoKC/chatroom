using Chat.Domain.Entities;
using Chat.Domain.Interfaces;
using Chat.Domain.Interfaces.Application;
using Chat.Domain.Interfaces.Persistence;
using MassTransit;

namespace Chat.UI.Consumers
{
    public class StockQuoteConsumer : IConsumer<StockQuote>
    {
        private readonly ILogger<StockQuoteConsumer> _logger;
        private readonly IChatService _chatService;
        private readonly IChatRepository _chatRepository;

        public StockQuoteConsumer(ILogger<StockQuoteConsumer> logger, IChatService chatService, IChatRepository chatRepository)
        {
            _logger = logger;
            _chatService = chatService;
            _chatRepository = chatRepository;
        }

        public async Task Consume(ConsumeContext<StockQuote> context)
        {
            try
            {
                _logger.LogInformation($"Message: {context}");

                var message = context.Message;

                if (message == null) return;
               
                using (var client = new HttpClient())
                {
                    var stockCode = message.StockCode;

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

                            var chatMessage = new ChatMessage()
                            {
                                MessageId = Guid.NewGuid(),
                                Message = messageFormat,
                                SentDate = DateTime.Now,
                                SenderUserId = message.SenderUserId
                            };

                            await _chatRepository.SaveAsync(chatMessage);

                            //await _chatService.BotMessage(chatMessage.SentDate, chatMessage.Message, null);
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