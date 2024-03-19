using MoscowSleddingBot.Abstractions;
using Telegram.Bot;

namespace MoscowSleddingBot.Services;

public class ReceiverService: ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(ITelegramBotClient botClient,  UpdateHandler updateHandler, ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : base(botClient, updateHandler, logger) {}
}
