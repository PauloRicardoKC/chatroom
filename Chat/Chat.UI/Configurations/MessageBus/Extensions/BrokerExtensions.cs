using MassTransit;
using System.Diagnostics.CodeAnalysis;

namespace Chat.UI.Configurations.MessageBus
{
    [ExcludeFromCodeCoverage]
    public static class BrokerExtensions
    {        
        public static IServiceCollection AddBroker(
            this IServiceCollection services,
            Action<IBrokerConfigurator> configureBroker,
            Action<IConsumerRegistration> configureConsumers = null,
            Action<IDestinationQueueMapper> configureDestinationQueues = null,
            Action<IRequestClientRegistration> configureRequestClients = null)
        {
            if (configureBroker == null)
                throw new ArgumentNullException(nameof(configureBroker));

            services.AddScoped<IBrokerConfigurator, BrokerConfigurator>();

            var provider = services.BuildServiceProvider();

            var broker = provider.GetService<IBrokerConfigurator>();
            configureBroker.Invoke(broker);

            var options = broker.Options;
            services.AddSingleton(options);
            services.AddMassTransit(configure =>
            {
                configure.SetKebabCaseEndpointNameFormatter();
                configure.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(options.HostOptions.Host, h =>
                    {
                        h.RequestedChannelMax(options.HostOptions.RequestedChannelMax);
                        h.RequestedConnectionTimeout(options.HostOptions.RequestedConnectionTimeout);
                    });
                    cfg.ConfigureEndpoints(context);
                    cfg.PrefetchCount = options.PrefetchCount;

                    cfg.UseKillSwitch(switchOptions => switchOptions
                        .SetActivationThreshold(options.KillSwitchOptions.ActivationThreshold)
                        .SetTripThreshold(options.KillSwitchOptions.TripThreshold)
                        .SetRestartTimeout(s: options.KillSwitchOptions.RestartTimeout));
                });

                services.AddSingleton(configure);
                services.AddScoped<IConsumerRegistration, ConsumerRegistration>();
                services.AddScoped<IDestinationQueueMapper, DestinationQueueMapper>();
                services.AddScoped<IRequestClientRegistration, RequestClientRegistration>();

                provider = services.BuildServiceProvider();
              
                var consumerRegistration = provider.GetService<IConsumerRegistration>();
                configureConsumers?.Invoke(consumerRegistration);

                var destinationQueueMapper = provider.GetService<IDestinationQueueMapper>();
                configureDestinationQueues?.Invoke(destinationQueueMapper);

                var requestClientRegistration = provider.GetService<IRequestClientRegistration>();
                configureRequestClients?.Invoke(requestClientRegistration);
            });

            if (options.AddHostedService)
            {
                services.AddMassTransitHostedService();
            }

            return services;
        }
    }
}