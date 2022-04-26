using Chat.Application.Hubs;
using Chat.Domain.Interfaces.Application;
using NSubstitute;

namespace Chat.Test.Application.Hubs
{
    public class ChatHubTest
    {
        private readonly IChatService _service;
        private readonly ChatHub _hub;

        public ChatHubTest()
        {
            _service = Substitute.For<IChatService>();
            _hub = Substitute.For<ChatHub>();

            _hub = new ChatHub(_service);
        }
    }
}
