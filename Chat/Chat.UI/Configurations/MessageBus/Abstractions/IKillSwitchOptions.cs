namespace Chat.UI.Configurations.MessageBus
{
    public interface IKillSwitchOptions
    {
        /// <summary>
        ///     The number of messages that must be consumed before the kill switch activates.
        /// </summary>
        int ActivationThreshold { get; set; }

        /// <summary>
        ///     The percentage of failed messages that triggers the kill switch. Should be 0-100, but seriously like 5-10.
        /// </summary>
        double TripThreshold { get; set; }

        /// <summary>
        ///     The wait time before restarting the receive endpoint, in seconds
        /// </summary>
        int RestartTimeout { get; set; }
    }
}
