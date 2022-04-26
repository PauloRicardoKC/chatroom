using Chat.Application.Hubs;
using Chat.Domain.Interfaces.Application;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Chat.Test.Application.Hubs
{
    public class ChatHubTest
    {
        private readonly IChatService _service;
        private readonly ChatHub _hub;

        public ChatHubTest()
        {
            _service = Substitute.For<IChatService>();

            _hub = new ChatHub(_service);
        }

        [Fact]
        public async Task SuccessSendMessage()
        {
            await _service.SendMessageAsync(Arg.Any<HubCallerContext>(), Arg.Any<IHubCallerClients>(), Arg.Any<string>());

            await _hub.SendMessage("Test message.");

            await _service.Received(1).SendMessageAsync(Arg.Any<HubCallerContext>(), Arg.Any<IHubCallerClients>(), Arg.Any<string>());
        }

        [Fact]
        public async Task SuccessJoinedChat()
        {
            await _service.UserJoinedAsync(Arg.Any<HubCallerContext>(), Arg.Any<IHubCallerClients>());

            await _hub.Joined();

            await _service.Received(1).UserJoinedAsync(Arg.Any<HubCallerContext>(), Arg.Any<IHubCallerClients>());
        }

        [Fact]
        public async Task SuccessOnConnected()
        {
            _service.ConnectUser(Arg.Any<HubCallerContext>());

            await _hub.OnConnectedAsync();

            _service.Received(1).ConnectUser(Arg.Any<HubCallerContext>());
        }

        [Fact]
        public async Task SuccessOnDisconnected()
        {
            await _service.DisconnectUser(Arg.Any<HubCallerContext>(), Arg.Any<IHubCallerClients>());

            await _hub.OnDisconnectedAsync(new ArgumentException("User disconnected by expiration time."));

            await _service.Received(1).DisconnectUser(Arg.Any<HubCallerContext>(), Arg.Any<IHubCallerClients>());
        }
    }
}