using MassTransit.ExtensionsDependencyInjectionIntegration;
using System.Diagnostics.CodeAnalysis;

namespace Chat.UI.Configurations.MessageBus
{
    [ExcludeFromCodeCoverage]
    public class RequestClientRegistration : IRequestClientRegistration
    {
        private readonly IServiceCollectionBusConfigurator _busConfigurator;

        public RequestClientRegistration(IServiceCollectionBusConfigurator busConfigurator)
        {
            _busConfigurator = busConfigurator;
        }

        public IRequestClientRegistration Add<T>() where T : class
        {
            _busConfigurator.AddRequestClient<T>();
            return this;
        }
    }
}