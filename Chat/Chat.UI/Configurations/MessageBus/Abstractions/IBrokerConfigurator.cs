namespace Chat.UI.Configurations.MessageBus
{
    public interface IBrokerConfigurator
    {
        IMessageBrokerOptions Options { get; }

        IBrokerConfigurator SetBrokerOptions(Action<IMessageBrokerOptions> configureBrokerOptions);
    }
}
