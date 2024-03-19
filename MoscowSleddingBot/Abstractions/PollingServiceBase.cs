using MoscowSleddingBot.Interfaces;

namespace MoscowSleddingBot.Abstractions;

public abstract class PollingServiceBase<TReceiverService>: BackgroundService where TReceiverService: IReceiverService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    internal PollingServiceBase(IServiceProvider serviceProvider, ILogger<PollingServiceBase<TReceiverService>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting polling service at {date}", DateTime.Now);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();

                await receiver.ReceiveAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Polling failed, occured exception: {Exception}", ex);

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
            }
        }
    }

}