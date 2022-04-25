using System.Diagnostics.CodeAnalysis;

namespace Chat.UI.Configurations.MessageBus
{
    [ExcludeFromCodeCoverage]
    public class BrokerHostOptions : IBrokerHostOptions
    {
        public string Host { get; set; }
        public ushort HeartbeatInterval { get; set; } = 3;
        public ushort RequestedChannelMax { get; set; } = 12;
        public int RequestedConnectionTimeout { get; set; } = 60000;
    }
}