using System.Diagnostics.CodeAnalysis;

namespace Chat.UI.Configurations.MessageBus
{
    [ExcludeFromCodeCoverage]
    public class KillSwitchOptions : IKillSwitchOptions
    {
        public int ActivationThreshold { get; set; } = 10;
        public double TripThreshold { get; set; } = 0.15;
        public int RestartTimeout { get; set; } = 60;
    }
}