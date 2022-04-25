namespace Chat.UI.Configurations.MessageBus
{
    public interface IMessageBrokerOptions
    {
        /// <summary>
        ///     Specify the number of messages to prefetch from the message broker
        /// </summary>
        int PrefetchCount { get; set; }

        IBrokerHostOptions HostOptions { get; set; }

        IKillSwitchOptions KillSwitchOptions { get; set; }

        bool AddHostedService { get; set; }
    }
}