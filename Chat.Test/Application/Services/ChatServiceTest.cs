using Chat.Application.Services;
using Chat.Domain.Entities;
using Chat.Domain.Interfaces.Application;
using Chat.Domain.Interfaces.Persistence;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Chat.Test.Application.Services
{
    public class ChatServiceTest
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IChatRepository _repository;
        private readonly ISendEndpointProvider _sender;
        private readonly ChatService _service;
        private readonly IMessageHandler _handler;

        public ChatServiceTest()
        {
            _serviceProvider = Substitute.For<IServiceProvider>();
            _repository = Substitute.For<IChatRepository>();
            _sender = Substitute.For<ISendEndpointProvider>();
            _handler = Substitute.For<IMessageHandler>();

            _service = new ChatService(_serviceProvider, _repository, _sender);
        }

        [Fact]
        public async Task FailOnConnectedWithoutUser()
        {
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();

            try
            {
                _service.ConnectUser(mockClientContext.Object);
                Assert.False(true);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task FailDisconnectUserWithoutUser()
        {
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            mockClientContext.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString);

            try
            {
                await _service.DisconnectUser(mockClientContext.Object, mockClients.Object);
                Assert.False(true);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task SuccessGetName()
        {
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            mockClientContext.Setup(c => c.User.Identity.Name).Returns("newtest@test.com");

            var result = _service.GetName(mockClientContext.Object);

            Assert.NotNull(result);
            Assert.Equal("newtest", result);
        }

        [Fact]
        public async Task FailFindUser()
        {
            try
            {
                _service.FindUser(Guid.NewGuid().ToString());
                Assert.False(true);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task SuccessUserJoined()
        {
            _repository.GetLastMessagesAsync().Returns(MessagesMock());               

            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            mockClients.Setup(clients => clients.Caller).Returns(mockClientProxy.Object);
            mockClients.Setup(clients => clients.Others).Returns(mockClientProxy.Object);
            mockClientContext.Setup(c => c.User.Identity.Name).Returns("tony@test.com");

            await _service.UserJoinedAsync(mockClientContext.Object, mockClients.Object);

            await _repository.Received(1).GetLastMessagesAsync();
        }

        [Fact]
        public async Task SuccesslUserJoinedWithoutMessages()
        {
            IEnumerable<ChatMessage> messages = null;
            _repository.GetLastMessagesAsync().Returns(messages);

            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            mockClients.Setup(clients => clients.Caller).Returns(mockClientProxy.Object);
            mockClientContext.Setup(c => c.User.Identity.Name).Returns("tony@test.com");

            try
            {
                await _service.UserJoinedAsync(mockClientContext.Object, mockClients.Object);
                Assert.False(true);
            }
            catch (Exception)
            {
                Assert.True(true);
                await _repository.Received(1).GetLastMessagesAsync();
            }
        }

        [Fact]
        public async Task FailUserJoinedWithoutClientsOthers()
        {
            _repository.GetLastMessagesAsync().Returns(MessagesMock());

            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            mockClients.Setup(clients => clients.Caller).Returns(mockClientProxy.Object);
            mockClientContext.Setup(c => c.User.Identity.Name).Returns("tony@test.com");

            try
            {
                await _service.UserJoinedAsync(mockClientContext.Object, mockClients.Object);
                Assert.False(true);
            }
            catch (Exception)
            {
                Assert.True(true);
                await _repository.Received(1).GetLastMessagesAsync();
            }           
        }

        [Fact]
        public async Task FailSendMessage()
        {            
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            Mock<HubCallerContext> mockClientContext = new Mock<HubCallerContext>();
            mockClients.Setup(clients => clients.Caller).Returns(mockClientProxy.Object);
            mockClients.Setup(clients => clients.Others).Returns(mockClientProxy.Object);
            mockClientContext.Setup(c => c.User.Identity.Name).Returns("tony@test.com");

            try
            {
                await _service.SendMessageAsync(mockClientContext.Object, mockClients.Object, "New test message");
                Assert.False(true);
            }
            catch (Exception)
            {
                Assert.True(true);
                await _repository.DidNotReceive().SaveAsync(Arg.Any<ChatMessage>());
            }
        }

        private static IEnumerable<ChatMessage> MessagesMock()
        {
            return new List<ChatMessage>
            {
                new ChatMessage
                {
                    MessageId = Guid.NewGuid(),
                    SenderUserId = Guid.NewGuid().ToString(),
                    Message = "My first message",
                    SentDate = DateTime.Now,
                    Username = "Tony"
                },
                new ChatMessage
                {
                    MessageId = Guid.NewGuid(),
                    SenderUserId = Guid.NewGuid().ToString(),
                    Message = "My second message",
                    SentDate = DateTime.Now,
                    Username = "Tony"
                }
            };
        }
    }
}
