using MoscowSleddingBot.Abstractions;

namespace MoscowSleddingBot.Services;

/// <summary>
/// class for a request acceptance service
/// </summary>
public class PollingService: PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger) : base(serviceProvider, logger) {}
}