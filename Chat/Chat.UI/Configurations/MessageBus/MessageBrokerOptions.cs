using System.Diagnostics.CodeAnalysis;

namespace Chat.UI.Configurations.MessageBus
{
    [ExcludeFromCodeCoverage]
    internal class MessageBrokerOptions : IMessageBrokerOptions
    {
        public MessageBrokerOptions()
        {
            HostOptions = new BrokerHostOptions();
            KillSwitchOptions = new KillSwitchOptions();
        }

        public int PrefetchCount { get; set; } = 1;
        public IBrokerHostOptions HostOptions { get; set; }

        /// <summary>
        /// A Kill Switch monitors a receive endpoint and automatically stops and restarts the endpoint in the presence of consumer faults. The options
        /// can be configured to adjust the trip threshold, restart timeout, and exceptions that are observed by the kill switch. When configured on the bus,
        /// a kill switch is installed on every receive endpoint.
        /// </summary>
        public IKillSwitchOptions KillSwitchOptions { get; set; }

        public bool AddHostedService { get; set; } = false;
    }
}