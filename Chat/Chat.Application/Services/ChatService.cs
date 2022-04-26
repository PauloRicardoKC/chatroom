using Chat.Domain.Dtos;
using Chat.Domain.Entities;
using Chat.Domain.Interfaces;
using Chat.Domain.Interfaces.Application;
using Chat.Domain.Interfaces.Persistence;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Application.Services
{
    public class ChatService : IChatService
    {
        public IList<IMessageHandler> MessageHandlers { get; set; } = new List<IMessageHandler>();
        private readonly IServiceProvider _serviceProvider;
        private readonly IChatRepository _chatRepository;
        private readonly ISendEndpointProvider _sender;

        public ChatService(
            IServiceProvider serviceProvider,
            IChatRepository chatRepositor,
            ISendEndpointProvider sender)
        {
            _serviceProvider = serviceProvider;
            _chatRepository = chatRepositor;
            _sender = sender;
        }

        /// <summary>
        /// Find message handlers using reflection. 
        /// </summary>
        private void FindMessageHandlers()
        {
            // Search
            var handlers = typeof(ChatService).Assembly
                .GetTypes()
                .Where(type => type.IsClass
                               && !type.IsAbstract
                               && type.IsPublic
                               && type.GetInterfaces().Any(i => i == typeof(IMessageHandler)))
                .ToList();

            // Instances
            foreach (var handler in handlers)
                MessageHandlers.Add(ActivatorUtilities.CreateInstance(_serviceProvider, handler) as IMessageHandler);

            // Ordering
            MessageHandlers = MessageHandlers.OrderBy(handler => handler.Order).ToList();
        }

        #region User Management

        /// <summary>
        /// Simple storage for user mappings.
        /// ConnectionId is used as key
        /// </summary>
        public static Dictionary<string, UserDetail> Users = new Dictionary<string, UserDetail>();

        /// <summary>
        /// Map Identity user to SignalR Id
        /// </summary>
        /// <param name="context">Hub context</param>
        public void ConnectUser(HubCallerContext context)
        {
            lock (Users)
            {
                Users.Add(context.ConnectionId, new UserDetail()
                {
                    FullName = context.User.Identity.Name,
                    Name = GetName(context, context.User.Identity.Name),
                    HubUserId = context.UserIdentifier,
                    ConnectionId = context.ConnectionId
                });
            }
        }

        /// <summary>
        /// Remove User mappings before disconnect
        /// </summary>
        /// <param name="context">Hub context</param>
        /// <param name="clients">Client collection</param>
        public async Task DisconnectUser(HubCallerContext context, IHubCallerClients clients)
        {
            // Dictionary is not thread safe!
            lock (Users)
            {
                if (Users.ContainsKey(context.ConnectionId))
                    Users.Remove(context.ConnectionId);
            }

            // Get current user name
            var name = GetName(context);

            // Notify other users that current user left
            await clients.Others.SendAsync("ReceiveMessage", DateTime.Now.ToString("G"), null, $"{name} left!");
        }

        /// <summary>
        /// Get`s user name from email
        /// </summary>
        /// <param name="context">Hub context</param>
        /// <param name="email">User email</param>
        /// <returns>Name before @ part</returns>
        public string GetName(HubCallerContext context, string email = null)
        {
            // '??=' is shortcut for 'email == null ? Identity.Name : email'
            email ??= context.User.Identity.Name;

            // User is forced by Authorization attribute, so it should not be null
            return email?.Split('@')[0];
        }

        /// <summary>
        /// Finds user details in mappings
        /// </summary>
        /// <param name="userId">User Name or FullName or HubId or ConnectionId</param>
        /// <returns></returns>
        public UserDetail FindUser(string userId)
        {
            lock (Users)
            {
                return Users.FirstOrDefault(user =>
                    user.Value.HubUserId.Equals(userId)
                    || user.Value.ConnectionId.Equals(userId)
                    || user.Value.Name.Equals(userId)
                    || user.Value.FullName.Equals(userId)
                ).Value;
            }
        }

        #endregion

        #region User Actions

        public async Task BotMessage(DateTime sentDate, string message, IHubCallerClients clients)
        {
            await clients.All.SendAsync("ReceiveMessage", sentDate.ToString("G"), "Bot", message);
        }

        /// <summary>
        /// Notifications when user joins chat
        /// Could be done in OnConnectedAsync event
        /// </summary>
        /// <param name="context">Hub context</param>
        /// <param name="clients">Client collection</param>
        public async Task UserJoinedAsync(HubCallerContext context, IHubCallerClients clients)
        {
            // Get current user name
            var name = GetName(context);

            var lastMessages = await _chatRepository.GetLastMessagesAsync();

            if (lastMessages.Any())
            {
                foreach (var message in lastMessages.OrderBy(x => x.SentDate))
                {                    
                    await clients.Caller.SendAsync("ReceiveMessage", message.SentDate.ToString("G"), message.Username.Split('@')[0], message.Message);
                }
            }
            
            // Notify other users that someone is here
            await clients.Others.SendAsync("ReceiveMessage", DateTime.Now.ToString("G"), null, $"{name} entered the chat room!");

            // Notify current user that we are ready
            await clients.Caller.SendAsync("ReceiveMessage", DateTime.Now.ToString("G"), name, "All ready to start!");
        }

        /// <summary>
        /// Handle user incoming messages
        /// </summary>
        /// <param name="context">Hub context</param>
        /// <param name="clients">Client collection</param>
        public async Task SendMessageAsync(HubCallerContext context, IHubCallerClients clients, string message)
        {
            // Load handlers (Dual lock check for safety)
            if (!MessageHandlers.Any())
                lock (MessageHandlers)
                    if (!MessageHandlers.Any())
                        FindMessageHandlers();

            // Let`s go though all handlers till we have success
            foreach (var handler in MessageHandlers)
            {
                if (await handler.Handle(context, clients, message))
                    break;
            }

            var senderUserId = context.UserIdentifier;

            if (message.Contains("/stock="))
            {
                var stockCode = message.Split('=')[1];

                await _sender.Send<StockQuote>(new
                {
                    senderUserId,
                    stockCode
                });
            }
            else
            {
                var chatMessage = new ChatMessage()
                {
                    MessageId = Guid.NewGuid(),
                    Message = message,
                    SentDate = DateTime.Now,
                    SenderUserId = context.UserIdentifier
                };

                await _chatRepository.SaveAsync(chatMessage);
            }
        }

        /// <summary>
        /// Gets connected users list formatted for chat output
        /// </summary>
        /// <param name="context">Hub context</param>
        /// <param name="clients">Client collection</param>
        /// <returns>User list formatted string</returns>
        public string GetUsersListString(HubCallerContext context, IHubCallerClients clients)
        {
            lock (Users)
            {
                return string.Join('\n', Users.Select(u => u.Value));
            }
        }
        #endregion
    }
}