using MoscowSleddingBot.Abstractions;
using Telegram.Bot;

namespace MoscowSleddingBot.Services;

/// <summary>
/// Class for the request acceptance service - updates from telegram
/// </summary>
public class ReceiverService: ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(ITelegramBotClient botClient,  UpdateHandler updateHandler, ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : base(botClient, updateHandler, logger) {}
}
