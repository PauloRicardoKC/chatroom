using MassTransit;
using System.Diagnostics.CodeAnalysis;

namespace Chat.UI
{
    [ExcludeFromCodeCoverage]
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBusControl _bus;

        public Worker(ILogger<Worker> logger, IBusControl bus)
        {
            _logger = logger;
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _bus.StartAsync(stoppingToken);
            _logger.LogInformation("Bus is started at {Address} running at: {Time}", _bus.Address.Authority,
                DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(120000, stoppingToken);
            }

            await _bus.StopAsync(stoppingToken);
            _logger.LogInformation("Bus is stopped at {Time}", DateTimeOffset.Now);
        }
    }
}