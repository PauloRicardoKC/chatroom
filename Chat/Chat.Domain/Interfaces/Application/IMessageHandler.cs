using Microsoft.AspNetCore.SignalR;

namespace Chat.Domain.Interfaces.Application
{
    public interface IMessageHandler
    {
        /// <summary>
        /// Handler order in processing
        /// </summary>
        uint Order { get; }

        /// <summary>
        /// Handles message. If false, we will use another handler.
        /// </summary>
        /// <param name="context">Hub context</param>
        /// <param name="clients">Client collection</param>
        /// <param name="message">User message</param>
        /// <returns></returns>
        Task<bool> Handle(HubCallerContext context, IHubCallerClients clients, string message);
    }
}