using Chat.Application.Handlers;
using Chat.Application.Hubs;
using Chat.Application.Services;
using Chat.Domain.Interfaces.Application;
using Chat.Domain.Interfaces.Persistence;
using Chat.Infra.Data.DataBases;
using Chat.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Chat.UI.Configurations.Ioc
{
    [ExcludeFromCodeCoverage]
    public static class IocConfiguration
    {
        public static void AddIoC(this IServiceCollection services, IConfiguration configuration)
        {
            Applications(services);
            Repositories(services, configuration);
        }

        public static ApplicationDbContext GetDbContext(IConfiguration configuration, string connectionStringName)
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlServer(configuration.GetConnectionString(connectionStringName),
                    config => config.EnableRetryOnFailure(3))
                .Options;

            return new ApplicationDbContext(options);
        }

        public static void UseChat(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapHub<ChatHub>("/chathub");
        }

        private static void Applications(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMessageHandler, SimpleMessageHandler>();
            services.AddScoped<IMessageHandler, UserListMessageHandler>();
            services.AddScoped<IMessageHandler, PrivateMessageHandler>();
        }

        private static void Repositories(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IChatRepository>(provider => 
                new ChatRepository(GetDbContext(configuration, "DefaultConnection")));            
        }
    }
}