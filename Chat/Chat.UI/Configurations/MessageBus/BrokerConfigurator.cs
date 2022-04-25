using System.Diagnostics.CodeAnalysis;

namespace Chat.UI.Configurations.MessageBus
{
    [ExcludeFromCodeCoverage]
    internal class BrokerConfigurator : IBrokerConfigurator
    {
        public IMessageBrokerOptions Options { get; private set; }

        public IBrokerConfigurator SetBrokerOptions(Action<IMessageBrokerOptions> configureBrokerOptions)
        {
            IMessageBrokerOptions brokerOptions = new MessageBrokerOptions();
            configureBrokerOptions.Invoke(brokerOptions);

            if (string.IsNullOrWhiteSpace(brokerOptions.HostOptions.Host))
                throw new InvalidOperationException("Can not start the service without a valid Message Broker Host");

            Options = brokerOptions;

            return this;
        }
    }
}