namespace MoscowSleddingBot.Interfaces;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken cancellationToken);
}