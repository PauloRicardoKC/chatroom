using Chat.Domain.Entities;
using Chat.Domain.Interfaces;
using Chat.Domain.Interfaces.Persistence;
using Chat.Test.Mocks;
using Chat.UI.Consumers;
using Flurl.Http.Testing;
using MassTransit;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Chat.Test.UI.Consumers
{
    public class StockQuoteConsumerTest
    {
        private readonly IChatRepository _repository;
        private readonly LoggerMock<StockQuoteConsumer> _logger;
        private readonly StockQuoteConsumer _consumer;
        private readonly ConsumeContext<StockQuote> _context;

        public StockQuoteConsumerTest(ITestOutputHelper output)
        {
            _repository = Substitute.For<IChatRepository>();
            _logger = new LoggerMock<StockQuoteConsumer>(output);
            _context = Substitute.For<ConsumeContext<StockQuote>>();
            _context.CancellationToken.Returns(CancellationToken.None);

            _consumer = new StockQuoteConsumer(_logger, _repository);
        }

        [Fact]
        public async Task SuccessStockQuote()
        {
            _context.Message.Returns(StockQuoteMock());

            var httpClient = Substitute.For<IHttpClientFactory>();
            httpClient.CreateClient().Returns(Substitute.For<HttpClient>());
            httpClient.CreateClient(Arg.Any<string>())
                .Returns(new HttpClient { BaseAddress = new Uri("http://chat.test.com") });

            var responseStockQuote = 200;

            var httpTest = new HttpTest();
            httpTest.RespondWithJson(responseStockQuote, 200);

            await _repository.SaveAsync(Arg.Any<ChatMessage>());

            await _consumer.Consume(_context);

            Assert.DoesNotContain("Error processing message", _logger.LogMessage);
        }

        [Fact]
        public async Task EmptyMessage_QueueCompleted()
        {
            StockQuoteModelMock messageMock = null;
            _context.Message.Returns(messageMock);

            await _consumer.Consume(_context);

            Assert.Equal(LogLevel.Information, _logger.LastLogLevel);
            await _repository.DidNotReceive().SaveAsync(Arg.Any<ChatMessage>());
        }

        [Fact]
        public async Task FailConsumerStockQuote_ShouldReturnException()
        {
            _context.Message.Returns(StockQuoteEmptyMock());

            try
            {           
                await _consumer.Consume(_context);
                
                Assert.False(true);
            } 
            catch (Exception)
            {
                Assert.Equal(LogLevel.Error, _logger.LastLogLevel);
                Assert.Contains("Error processing message", _logger.LogMessage);
            }            
        }

        private static StockQuoteModelMock StockQuoteMock()
        {
            return new StockQuoteModelMock
            {
                SenderUserId = Guid.NewGuid().ToString(),
                StockCode = "aapl.us"
            };
        }

        private static StockQuoteModelMock StockQuoteEmptyMock()
        {
            return new StockQuoteModelMock
            {
                SenderUserId = Guid.NewGuid().ToString(),
                StockCode = ""
            };
        }

        internal class StockQuoteModelMock : StockQuote
        {
            public string SenderUserId { get; set; }
            public string StockCode { get; set; }
        }
    }
}