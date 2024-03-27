using MoscowSleddingBot.Interfaces;

namespace MoscowSleddingBot.Abstractions;

/// <summary>
/// An abstract class that creates the basis for a request acceptance service
/// </summary>
/// <typeparam name="TReceiverService">The generalized parameter is the interface of the request acceptance service class</typeparam>
public abstract class PollingServiceBase<TReceiverService>: BackgroundService where TReceiverService: IReceiverService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    internal PollingServiceBase(IServiceProvider serviceProvider, ILogger<PollingServiceBase<TReceiverService>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// The method of starting the polling service
    /// </summary>
    /// <param name="cancellationToken">Operation cancellation token</param>
    /// <returns></returns>
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