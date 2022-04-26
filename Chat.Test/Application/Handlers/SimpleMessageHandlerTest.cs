using Chat.Application.Handlers;
using Chat.Domain.Interfaces.Application;
using NSubstitute;

namespace Chat.Test.Application.Handlers
{
    public class SimpleMessageHandlerTest
    {
        private readonly IChatService _service;
        private readonly SimpleMessageHandler _handler;

        public SimpleMessageHandlerTest()
        {
            _service = Substitute.For<IChatService>();
            _handler = Substitute.For<SimpleMessageHandler>();

            _handler = new SimpleMessageHandler(_service);
        }
    }
}