using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using System.Diagnostics.CodeAnalysis;
using IRetryConfigurator = GreenPipes.Configurators.IRetryConfigurator;

namespace Chat.UI.Configurations.MessageBus
{
    [ExcludeFromCodeCoverage]
    public class ConsumerRegistration : IConsumerRegistration
    {
        private readonly IServiceCollectionBusConfigurator _busConfigurator;

        public ConsumerRegistration(IServiceCollectionBusConfigurator busConfigurator)
        {
            _busConfigurator = busConfigurator;
        }

        public IConsumerRegistration Add<T>(Action<IRetryConfigurator> configureRetries = null)
            where T : class, IConsumer
        {
            if (configureRetries is null)
                _busConfigurator.AddConsumer<T>(c =>
                {
                    c.UseInMemoryOutbox();
                });
            else
                _busConfigurator.AddConsumer<T>(c =>
                {
                    c.UseInMemoryOutbox();
                    c.UseMessageRetry(configureRetries.Invoke);
                });

            return this;
        }
    }
}