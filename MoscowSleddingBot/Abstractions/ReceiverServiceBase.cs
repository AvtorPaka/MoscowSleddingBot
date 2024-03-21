using MoscowSleddingBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace MoscowSleddingBot.Abstractions;

/// <summary>
/// An abstract class that creates the basis for the request acceptance service - updates from telegram
/// </summary>
/// <typeparam name="TUpdateHandler">The generalized parameter is the interface of the request processing class</typeparam>
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

    /// <summary>
    /// The method that starts the process of accepting http requests by the bot client
    /// </summary>
    /// <param name="cts">Operation cancellation token</param>
    /// <returns></returns>
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