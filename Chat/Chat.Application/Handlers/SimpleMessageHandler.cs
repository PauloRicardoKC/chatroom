using Chat.Domain.Interfaces.Application;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.Handlers
{
    public class SimpleMessageHandler : IMessageHandler
    {
        public uint Order => 1000;

        private readonly IChatService _chatService;

        public SimpleMessageHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<bool> Handle(HubCallerContext context, IHubCallerClients clients, string message)
        {
            //todo Should store message for later
            await clients.All.SendAsync("ReceiveMessage", DateTime.Now.ToString("G"), _chatService.GetName(context), message);

            return true;
        }
    }
}
