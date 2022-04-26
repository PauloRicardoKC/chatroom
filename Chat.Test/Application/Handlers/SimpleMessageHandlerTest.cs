using Chat.Application.Handlers;
using Chat.Domain.Interfaces.Application;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Chat.Test.Application.Handlers
{
    public class SimpleMessageHandlerTest
    {
        private readonly IChatService _service;
        private readonly SimpleMessageHandler _handler;

        public SimpleMessageHandlerTest()
        {
            _service = Substitute.For<IChatService>();

            _handler = new SimpleMessageHandler(_service);
        }

        [Fact]
        public async Task SuccessSimpleMessage()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            mockClientContext.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString);

            var result = await _handler.Handle(mockClientContext.Object, mockClients.Object, "Test message.");

            Assert.True(result);
        }
    }
}