using Telegram.Bot;
using Telegram.Bot.Types;

namespace MoscowSleddingBot.Interfaces;

public interface IMessageService
{
    Task BotOnMessageReceived(Message message, CancellationToken cancellationToken);
}