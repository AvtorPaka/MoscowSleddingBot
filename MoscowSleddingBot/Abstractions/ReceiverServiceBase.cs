using MoscowSleddingBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace MoscowSleddingBot.Abstractions;

public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService where TUpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateHandler _updateHandler;
    private readonly ILogger<ReceiverServiceBase<TUpdateHandler>> _logger;

    internal ReceiverServiceBase( ITelegramBotClient botClient, TUpdateHandler updateHandler, ILogger<ReceiverServiceBase<TUpdateHandler>> logger)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    public async Task ReceiveAsync(CancellationToken cts)
    {
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new UpdateType[] { UpdateType.Message, UpdateType.EditedMessage, UpdateType.CallbackQuery },
            ThrowPendingUpdates = true
        };

        var me = await _botClient.GetMeAsync(cts);
        _logger.LogInformation("Start receiving updates for {BotName} at {date}", me.Username ?? "MoscowSleddingBot", DateTime.Now);

        await _botClient.ReceiveAsync(
            updateHandler: _updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: cts);
    }
}