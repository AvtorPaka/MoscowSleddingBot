using Telegram.Bot;

namespace MoscowSleddingBot.Interfaces;

public interface IErrorHandler
{
    Task HandlePollingErrorAsync(ITelegramBotClient telegramBotClient, Exception exception, CancellationToken cancellationToken);
}