namespace Chat.UI.Configurations.MessageBus
{
    public interface IBrokerHostOptions
    {
        /// <summary>
        ///     The host name of the broker, or a well-formed URI host address
        /// </summary>
        string Host { get; set; }

        /// <summary>
        ///     Specifies the heartbeat interval, in seconds, used to maintain the connection to RabbitMQ.
        ///     Setting this value to zero will disable heartbeats, allowing the connection to timeout
        ///     after an inactivity period.
        /// </summary>
        ushort HeartbeatInterval { get; set; }

        /// <summary>
        ///     Set the maximum number of channels allowed for the connection
        /// </summary>
        ushort RequestedChannelMax { get; set; }

        /// <summary>
        ///     The requested connection timeout, in milliseconds
        /// </summary>
        int RequestedConnectionTimeout { get; set; }
    }
}
