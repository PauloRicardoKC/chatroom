using GreenPipes.Configurators;
using MassTransit;

namespace Chat.UI.Configurations.MessageBus
{
    public interface IConsumerRegistration
    {
        IConsumerRegistration Add<T>(Action<IRetryConfigurator> configureRetries = null) where T : class, IConsumer;
    }
}