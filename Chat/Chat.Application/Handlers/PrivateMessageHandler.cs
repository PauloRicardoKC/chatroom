using Chat.Domain.Interfaces.Application;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Chat.Application.Handlers
{
    [ExcludeFromCodeCoverage]
    public class PrivateMessageHandler : IMessageHandler
    {
        public uint Order => 100;

        private readonly IChatService _chatService;

        public PrivateMessageHandler(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<bool> Handle(HubCallerContext context, IHubCallerClients clients, string message)
        {
            if (!message.StartsWith("/pm:", StringComparison.InvariantCultureIgnoreCase))
                return false;

            // Bit of Regex, message is in format '/pm: userId message'
            var regex = new Regex(@"^\/pm\:\s*([^\s]+)\s+(.*)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var match = regex.Match(message);
            if (match.Success)
            {
                // Lets try to send message
                var userId = match.Groups[1].Value;

                // Find our user
                var found = _chatService.FindUser(userId);
                if (found != null)
                {
                    userId = found.HubUserId;
                    var userName = found.Name;

                    // Should store message for later
                    await clients.Users(userId, context.UserIdentifier).SendAsync("ReceiveMessage", DateTime.Now.ToString("G"), $"{_chatService.GetName(context)} -> {userName}", match.Groups[2].Value);

                    return true;
                }
            }

            // Fail
            await clients.Caller.SendAsync("ReceiveMessage", DateTime.Now.ToString("G"), null, "Wrong command or user :/");

            return true; // We don`t want other users notice even if we fail in sending private message
        }
    }
}
