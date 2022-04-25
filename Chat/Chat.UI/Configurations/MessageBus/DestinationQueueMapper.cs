using MassTransit;
using MassTransit.Definition;
using System.Diagnostics.CodeAnalysis;

namespace Chat.UI.Configurations.MessageBus
{
    [ExcludeFromCodeCoverage]
    public class DestinationQueueMapper : IDestinationQueueMapper
    {
        public IDestinationQueueMapper Map<T>() where T : class
        {
            var interfaceQueueName = typeof(T).Name;
            var queueName = KebabCaseEndpointNameFormatter.Instance.SanitizeName(interfaceQueueName);

            EndpointConvention.Map<T>(new Uri($"queue:{queueName}"));

            return this;
        }
    }
}