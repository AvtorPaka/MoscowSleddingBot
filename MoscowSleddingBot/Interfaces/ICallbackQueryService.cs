using Telegram.Bot;
using Telegram.Bot.Types;

namespace MoscowSleddingBot.Interfaces;

public interface ICallbackQueryService
{
    Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken);
}